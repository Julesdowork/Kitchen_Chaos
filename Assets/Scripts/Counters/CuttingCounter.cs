using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    // Remember to reset this and all other static events
    public static event EventHandler OnAnyCut;

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] _cuttingRecipeSOs;

    private int _cuttingProgress;

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
                    // Player carrying something that can be cut
                    KitchenObject kitchenObject = player.GetKitchenObject();
                    kitchenObject.KitchenObjectParent = this;

                    InteractLogicPlaceObjectOnCounterServerRpc();
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
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());
                    }
                }
            }
            else
            {
                // Player isn't carrying anything
                GetKitchenObject().KitchenObjectParent = player;
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicPlaceObjectOnCounterServerRpc()
    {
        InteractLogicPlaceObjectOnCounterClientRpc();
    }

    [ClientRpc]
    private void InteractLogicPlaceObjectOnCounterClientRpc()
    {
        _cuttingProgress = 0;

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = 0
        });
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().KitchenObjectSO))
        {
            // There is a KitchenObject here AND it can be cut
            CutObjectServerRpc();
            TestCuttingProgressDoneServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void CutObjectServerRpc()
    {
        CutObjectClientRpc();
    }

    [ClientRpc]
    private void CutObjectClientRpc()
    {
        _cuttingProgress++;

        OnCut?.Invoke(this, EventArgs.Empty);
        OnAnyCut?.Invoke(this, EventArgs.Empty);

        CuttingRecipeSO recipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().KitchenObjectSO);

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = (float)_cuttingProgress / recipeSO.CuttingProgressMax
        });
    }

    [ServerRpc(RequireOwnership = false)]
    private void TestCuttingProgressDoneServerRpc()
    {
        CuttingRecipeSO recipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().KitchenObjectSO);

        if (_cuttingProgress >= recipeSO.CuttingProgressMax)
        {
            KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().KitchenObjectSO);

            KitchenObject.DestroyKitchenObject(GetKitchenObject());

            KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO recipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return recipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO recipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (recipeSO != null)
        {
            return recipeSO.Output;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO recipeSO in _cuttingRecipeSOs)
        {
            if (recipeSO.Input == inputKitchenObjectSO)
            {
                return recipeSO;
            }
        }
        return null;
    }
}
