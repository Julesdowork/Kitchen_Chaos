using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;
    [SerializeField] private Transform _topPoint;

    public void Interact()
    {
        Debug.Log("Interact");
        Transform kitchenObject = Instantiate(_kitchenObjectSO.Prefab, _topPoint);
        kitchenObject.localPosition = Vector3.zero;

        Debug.Log(kitchenObject.GetComponent<KitchenObject>().KitchenObjectSO.ObjectName);
    }
}
