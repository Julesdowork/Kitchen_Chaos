using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CuttingRecipeSO : ScriptableObject
{
    [SerializeField] private KitchenObjectSO input;
    [SerializeField] private KitchenObjectSO output;
    [SerializeField] private int cuttingProgressMax;

    public KitchenObjectSO Input => input;
    public KitchenObjectSO Output => output;
    public int CuttingProgressMax => cuttingProgressMax;
}
