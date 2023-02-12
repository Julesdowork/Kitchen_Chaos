using System;
using System.Collections;
using System.Collections.Generic;
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

    void Awake()
    {
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
            _kitchenObjectSOs.Add(kitchenObjectSO);

            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                kitchenObjectSO = kitchenObjectSO
            });

            return true;
        }
    }
}
