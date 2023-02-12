using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu]
public class RecipeListSO : ScriptableObject
{
    [SerializeField] private List<RecipeSO> recipeSOs;

    public List<RecipeSO> RecipeSOs => recipeSOs;
}
