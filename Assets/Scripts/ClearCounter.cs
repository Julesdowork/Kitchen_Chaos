using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;
    [SerializeField] private Transform _topPoint;

    private KitchenObject _kitchenObject;

    public KitchenObject KitchenObject
    {
        get => _kitchenObject;
        set => _kitchenObject = value;
    }

    public void Interact(Player player)
    {
        if (_kitchenObject == null)
        {
            Transform kitchenObject = Instantiate(_kitchenObjectSO.Prefab, _topPoint);
            kitchenObject.GetComponent<KitchenObject>().KitchenObjectParent = this;
        }
        else
        {
            // Give the object to the player
            _kitchenObject.KitchenObjectParent = player;
        }
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return _topPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return _kitchenObject != null;
    }
}
