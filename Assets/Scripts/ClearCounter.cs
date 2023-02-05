using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;
    [SerializeField] private Transform _topPoint;
    [SerializeField] private ClearCounter _secondClearCounter;
    [SerializeField] private bool _testing;

    private KitchenObject _kitchenObject;

    public KitchenObject KitchenObject
    {
        get => _kitchenObject;
        set => _kitchenObject = value;
    }

    void Update()
    {
        if (_testing && Input.GetKeyDown(KeyCode.T))
        {
            if (_kitchenObject != null)
            {
                _kitchenObject.ClearCounter = _secondClearCounter;
            }
        }
    }

    public void Interact()
    {
        if (_kitchenObject == null)
        {
            Transform kitchenObject = Instantiate(_kitchenObjectSO.Prefab, _topPoint);
            kitchenObject.GetComponent<KitchenObject>().ClearCounter = this;
        }
        else
        {
            Debug.Log(_kitchenObject.ClearCounter);
        }
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return _topPoint;
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
