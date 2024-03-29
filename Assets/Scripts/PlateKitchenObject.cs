using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> _validKitchenObjectSOs;

    private List<KitchenObjectSO> _kitchenObjectSOs;

    public List<KitchenObjectSO> KitchenObjectSOs => _kitchenObjectSOs;

    protected override void Awake()
    {
        base.Awake();
        _kitchenObjectSOs= new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!_validKitchenObjectSOs.Contains(kitchenObjectSO))
        {
            // Not a valid ingredient!
            return false;
        }
        if (_kitchenObjectSOs.Contains(kitchenObjectSO))
        {
            // Already contains this ingredient
            return false;
        }
        else
        {
            AddIngredientServerRpc(
                GameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObjectSO)
            );

            return true;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddIngredientServerRpc(int kitchenObjectSOIndex)
    {
        AddIngredientClientRpc(kitchenObjectSOIndex);
    }

    [ClientRpc]
    private void AddIngredientClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectSO kitchenObjectSO = GameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        _kitchenObjectSOs.Add(kitchenObjectSO);

        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
        {
            kitchenObjectSO = kitchenObjectSO
        });
    }
}
