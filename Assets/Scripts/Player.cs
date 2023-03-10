using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour, IKitchenObjectParent
{
    //public static Player Instance { get; private set; }

    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private LayerMask _countersLayer;
    [SerializeField] private Transform _kitchenObjectHoldPoint;

    private bool _isWalking;
    private Vector3 _lastInteractDir;
    private BaseCounter _selectedCounter;
    private KitchenObject _kitchenObject;

    void Awake()
    {
        //Instance = this;
    }

    void Start()
    {
        GameInput.Instance.OnInteract += GameInput_OnInteract;
        GameInput.Instance.OnInteractAlternate += GameInput_OnInteractAlternate;
    }

    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        HandleMovement();
        HandleInteractions();
    }

    private void GameInput_OnInteract(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;

        if (_selectedCounter != null)
        {
            _selectedCounter.Interact(this);
        }
    }

    private void GameInput_OnInteractAlternate(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;

        if (_selectedCounter != null)
        {
            _selectedCounter.InteractAlternate(this);
        }
    }

    public bool IsWalking()
    {
        return _isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            _lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, _lastInteractDir, out RaycastHit hit, interactDistance, _countersLayer))
        {
            if (hit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // Has ClearCounter
                if (baseCounter != _selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = _moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // Cannot move towards this direction

            // Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = (moveDir.x < -0.5f || moveDir.x > 0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                // Can move only on the X
                moveDir = moveDirX;
            }
            else
            {
                // Cannot move only on the X

                // Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -0.5f || moveDir.z > 0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    // Can move only on the Z
                    moveDir = moveDirZ;
                }
                else
                {
                    // Cannot move in any direction
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }
        
        _isWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    private void SetSelectedCounter(BaseCounter baseCounter)
    {
        _selectedCounter = baseCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            selectedCounter = _selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return _kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
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
