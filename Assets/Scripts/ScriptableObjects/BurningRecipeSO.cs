using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BurningRecipeSO : ScriptableObject
{
    [SerializeField] private KitchenObjectSO input;
    [SerializeField] private KitchenObjectSO output;
    [SerializeField] private float burningTimerMax;

    public KitchenObjectSO Input => input;
    public KitchenObjectSO Output => output;
    public float BurningTimerMax => burningTimerMax;
}
