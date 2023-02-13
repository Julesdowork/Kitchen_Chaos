using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteract;
    public event EventHandler OnInteractAlternate;
    public event EventHandler OnPause;

    private PlayerInputActions _inputActions;

    void Awake()
    {
        Instance = this;

        _inputActions = new PlayerInputActions();
        _inputActions.Player.Enable();

        _inputActions.Player.Interact.performed += Interact_performed;
        _inputActions.Player.InteractAlternate.performed += InteractAlt_performed;
        _inputActions.Player.Pause.performed += Pause_performed;
    }

    void OnDestroy()
    {
        _inputActions.Player.Interact.performed -= Interact_performed;
        _inputActions.Player.InteractAlternate.performed -= InteractAlt_performed;
        _inputActions.Player.Pause.performed -= Pause_performed;

        _inputActions.Dispose();
    }

    private void Pause_performed(InputAction.CallbackContext obj)
    {
        OnPause?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlt_performed(InputAction.CallbackContext obj)
    {
        OnInteractAlternate?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnInteract?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = _inputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }
}
