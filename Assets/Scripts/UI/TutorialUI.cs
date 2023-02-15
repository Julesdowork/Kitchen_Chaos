using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _keyMoveUpText;
    [SerializeField] private TextMeshProUGUI _keyMoveDownText;
    [SerializeField] private TextMeshProUGUI _keyMoveLeftText;
    [SerializeField] private TextMeshProUGUI _keyMoveRightText;
    [SerializeField] private TextMeshProUGUI _keyInteractText;
    [SerializeField] private TextMeshProUGUI _keyInteractAltText;
    [SerializeField] private TextMeshProUGUI _keyPauseText;
    [SerializeField] private TextMeshProUGUI _keyGamepadInteractText;
    [SerializeField] private TextMeshProUGUI _keyGamepadInteractAltText;
    [SerializeField] private TextMeshProUGUI _keyGamepadPauseText;

    void Start()
    {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        
        UpdateVisual();
        
        Show();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void GameInput_OnBindingRebind(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        _keyMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        _keyMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        _keyMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        _keyMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        _keyInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        _keyInteractAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact_Alt);
        _keyPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        _keyGamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
        _keyGamepadInteractAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact_Alt);
        _keyGamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
