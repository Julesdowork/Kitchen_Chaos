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

    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        Interact_Alt,
        Pause,
        Gamepad_Interact,
        Gamepad_Interact_Alt,
        Gamepad_Pause
    }

    private const string BINDINGS_KEY = "InputBindings";

    private PlayerInputActions _inputActions;

    void Awake()
    {
        Instance = this;

        _inputActions = new PlayerInputActions();

        if (PlayerPrefs.HasKey(BINDINGS_KEY))
        {
            _inputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(BINDINGS_KEY));
        }

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

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                return _inputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return _inputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return _inputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return _inputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return _inputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.Interact_Alt:
                return _inputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return _inputActions.Player.Pause.bindings[0].ToDisplayString();
            case Binding.Gamepad_Interact:
                return _inputActions.Player.Interact.bindings[1].ToDisplayString();
            case Binding.Gamepad_Interact_Alt:
                return _inputActions.Player.InteractAlternate.bindings[1].ToDisplayString();
            case Binding.Gamepad_Pause:
                return _inputActions.Player.Pause.bindings[1].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        _inputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = _inputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = _inputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = _inputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = _inputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = _inputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.Interact_Alt:
                inputAction = _inputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = _inputActions.Player.Pause;
                bindingIndex = 0;
                break;
            case Binding.Gamepad_Interact:
                inputAction = _inputActions.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_Interact_Alt:
                inputAction = _inputActions.Player.InteractAlternate;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_Pause:
                inputAction = _inputActions.Player.Pause;
                bindingIndex = 1;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();     // need to do this manually in some versions of Unity
                _inputActions.Player.Enable();
                onActionRebound();

                PlayerPrefs.SetString(BINDINGS_KEY, _inputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
            })
            .Start();
    }
}
