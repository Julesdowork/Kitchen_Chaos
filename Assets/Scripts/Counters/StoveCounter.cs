using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    [SerializeField] private FryingRecipeSO[] _fryingRecipeSOs;
    [SerializeField] private BurningRecipeSO[] _burningRecipeSOs;

    private NetworkVariable<State> _state = new NetworkVariable<State>(State.Idle);
    private NetworkVariable<float> _fryingTimer = new NetworkVariable<float>(0);
    private NetworkVariable<float> _burningTimer = new NetworkVariable<float>(0);
    private FryingRecipeSO _fryingRecipeSO;
    private BurningRecipeSO _burningRecipeSO;

    public override void OnNetworkSpawn()
    {
        _fryingTimer.OnValueChanged += FryingTimer_OnValueChanged;
        _burningTimer.OnValueChanged += BurningTimer_OnValueChanged;
        _state.OnValueChanged += State_OnValueChanged;
    }

    private void FryingTimer_OnValueChanged(float previousValue, float newValue)
    {
        float fryingTimerMax = _fryingRecipeSO != null ? _fryingRecipeSO.FryingTimerMax : 1f;

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = _fryingTimer.Value / fryingTimerMax
        });
    }

    private void State_OnValueChanged(State previousState, State newState)
    {
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            state = _state.Value
        });

        if (_state.Value == State.Burned || _state.Value == State.Idle)
        {
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = 0
            });
        }
    }

    private void BurningTimer_OnValueChanged(float previousValue, float newValue)
    {
        float burningTimerMax = _burningRecipeSO != null ? _burningRecipeSO.BurningTimerMax : 1f;

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = _burningTimer.Value / burningTimerMax
        });
    }

    void Update()
    {
        if (GameManager.Instance.IsGameOver())
            return;

        if (!IsServer)
            return;

        if (HasKitchenObject())
        {
            switch (_state.Value)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    _fryingTimer.Value += Time.deltaTime;

                    if (_fryingTimer.Value > _fryingRecipeSO.FryingTimerMax)
                    {
                        // Fried
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());

                        KitchenObject.SpawnKitchenObject(_fryingRecipeSO.Output, this);

                        _state.Value = State.Fried;
                        _burningTimer.Value = 0;
                        SetBurningRecipeSOClientRpc(
                            GameMultiplayer.Instance.GetKitchenObjectSOIndex(GetKitchenObject().KitchenObjectSO)
                        );
                    }
                    break;
                case State.Fried:
                    _burningTimer.Value += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = _burningTimer.Value / _burningRecipeSO.BurningTimerMax
                    });

                    if (_burningTimer.Value > _burningRecipeSO.BurningTimerMax)
                    {
                        // Burned
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());

                        KitchenObject.SpawnKitchenObject(_burningRecipeSO.Output, this);
                        _state.Value = State.Burned;
                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // There is no KitchenObject here
            if (player.HasKitchenObject())
            {
                // Player is carrying something
                if (HasRecipeWithInput(player.GetKitchenObject().KitchenObjectSO))
                {
                    // Player carrying something that can be fried
                    KitchenObject kitchenObject = player.GetKitchenObject();
                    kitchenObject.KitchenObjectParent = this;

                    InteractLogicPlaceObjectOnCounterServerRpc(
                        GameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObject.KitchenObjectSO)
                    );
                }
            }
            else
            {
                // Player isn't carrying anything
            }
        }
        else
        {
            // There is a KitchenObject here
            if (player.HasKitchenObject())
            {
                // Player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player is holding a Plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().KitchenObjectSO))
                    {
                        GetKitchenObject().DestroySelf();

                        _state.Value = State.Idle;
                    }
                }
            }
            else
            {
                // Player isn't carrying anything
                GetKitchenObject().KitchenObjectParent = player;

                SetStateIdleServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetStateIdleServerRpc()
    {
        _state.Value = State.Idle;
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicPlaceObjectOnCounterServerRpc(int kitchenObjectSOIndex)
    {
        _fryingTimer.Value = 0;
        _state.Value = State.Frying;

        SetFryingRecipeSOClientRpc(kitchenObjectSOIndex);
    }

    [ClientRpc]
    private void SetFryingRecipeSOClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectSO kitchenObjectSO = GameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);

        _fryingRecipeSO = GetFryingRecipeSOWithInput(kitchenObjectSO);
    }

    [ClientRpc]
    private void SetBurningRecipeSOClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectSO kitchenObjectSO = GameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);

        _burningRecipeSO = GetBurningRecipeSOWithInput(kitchenObjectSO);
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO recipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return recipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO recipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (recipeSO != null)
        {
            return recipeSO.Output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in _fryingRecipeSOs)
        {
            if (fryingRecipeSO.Input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in _burningRecipeSOs)
        {
            if (burningRecipeSO.Input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }

    public bool IsFried()
    {
        return _state.Value == State.Fried;
    }
}
