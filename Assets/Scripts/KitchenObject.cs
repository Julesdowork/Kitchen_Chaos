using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class KitchenObject : NetworkBehaviour
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    private IKitchenObjectParent _kitchenObjectParent;
    private FollowTransform _followTransform;

    protected virtual void Awake()
    {
        _followTransform = GetComponent<FollowTransform>();
    }

    public KitchenObjectSO KitchenObjectSO { get { return _kitchenObjectSO; } }

    public IKitchenObjectParent KitchenObjectParent
    {
        get
        {
            return _kitchenObjectParent;
        }
        set
        {
            SetKitchenObjectParentServerRpc(value.GetNetworkObject());
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetKitchenObjectParentServerRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        SetKitchenObjectParentClientRpc(kitchenObjectParentNetworkObjectReference);
    }

    [ClientRpc]
    private void SetKitchenObjectParentClientRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
        IKitchenObjectParent kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();

        if (_kitchenObjectParent != null)
        {
            _kitchenObjectParent.ClearKitchenObject();
        }

        _kitchenObjectParent = kitchenObjectParent;

        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("IKitchenObjectParent already has a kitchen object.");
        }
        kitchenObjectParent.SetKitchenObject(this);

        _followTransform.SetTargetTransform(kitchenObjectParent.GetKitchenObjectFollowTransform());
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
