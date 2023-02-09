using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabObject;

    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            Transform kitchenObject = Instantiate(_kitchenObjectSO.Prefab);
            kitchenObject.GetComponent<KitchenObject>().KitchenObjectParent = player;
            OnPlayerGrabObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
