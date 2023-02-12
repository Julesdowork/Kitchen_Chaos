using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RecipeSO : ScriptableObject
{
    [SerializeField] private List<KitchenObjectSO> kitchenObjectSOs;
    [SerializeField] private string recipeName;

    public List<KitchenObjectSO> KitchenObjectSOs => kitchenObjectSOs;
    public string RecipeName => recipeName;
}
