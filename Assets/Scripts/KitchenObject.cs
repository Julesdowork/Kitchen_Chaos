using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenObject : NetworkBehaviour
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    private IKitchenObjectParent _kitchenObjectParent;

    public KitchenObjectSO KitchenObjectSO { get { return _kitchenObjectSO; } }

    public IKitchenObjectParent KitchenObjectParent
    {
        get
        {
            return _kitchenObjectParent;
        }
        set
        {
            if (_kitchenObjectParent != null)
            {
                _kitchenObjectParent.ClearKitchenObject();
            }

            _kitchenObjectParent = value;

            if (value.HasKitchenObject())
            {
                Debug.LogError("IKitchenObjectParent already has a kitchen object.");
            }

            value.SetKitchenObject(this);

            //transform.parent = value.GetKitchenObjectFollowTransform();
            //transform.localPosition = Vector3.zero;
        }
    }

    public void DestroySelf()
    {
        _kitchenObjectParent.ClearKitchenObject();

        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }

    public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        GameMultiplayer.Instance.SpawnKitchenObject(kitchenObjectSO, kitchenObjectParent);
    }
}
