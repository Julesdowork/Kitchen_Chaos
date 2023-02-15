using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeliveryResultUI : MonoBehaviour
{
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _message;
    [SerializeField] private Color _successColor;
    [SerializeField] private Color _failedColor;
    [SerializeField] private Sprite _successSprite;
    [SerializeField] private Sprite _failedSprite;

    private const string POPUP = "Popup";

    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

        gameObject.SetActive(false);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        _animator.SetTrigger(POPUP);
        _backgroundImage.color = _failedColor;
        _icon.sprite = _failedSprite;
        _message.text = "DELIVERY\nFAILED";
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        _animator.SetTrigger(POPUP);
        _backgroundImage.color = _successColor;
        _icon.sprite = _successSprite;
        _message.text = "DELIVERY\nSUCCESS";
    }
}
