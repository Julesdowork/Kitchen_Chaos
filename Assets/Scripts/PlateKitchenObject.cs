using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
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
            return true;
        }
    }
}
