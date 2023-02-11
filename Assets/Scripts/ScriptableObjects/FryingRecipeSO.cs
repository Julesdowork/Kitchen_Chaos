using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FryingRecipeSO : ScriptableObject
{
    [SerializeField] private KitchenObjectSO input;
    [SerializeField] private KitchenObjectSO output;
    [SerializeField] private float fryingTimerMax;

    public KitchenObjectSO Input => input;
    public KitchenObjectSO Output => output;
    public float FryingTimerMax => fryingTimerMax;
}
