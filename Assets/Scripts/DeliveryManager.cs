using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    [SerializeField] private RecipeListSO _recipeListSO;

    private List<RecipeSO> _waitingRecipeSOs;
    private float _spawnRecipeTimer;
    private float _spawnRecipeTimerMax = 4f;
    private int _waitingRecipesMax = 4;

    public List<RecipeSO> WaitingRecipeSOs => _waitingRecipeSOs;

    void Awake()
    {
        Instance = this;

        _waitingRecipeSOs = new List<RecipeSO>();
    }

    void Update()
    {
        _spawnRecipeTimer -= Time.deltaTime;
        if (_spawnRecipeTimer <= 0)
        {
            _spawnRecipeTimer = _spawnRecipeTimerMax;

            if (_waitingRecipeSOs.Count < _waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = _recipeListSO.RecipeSOs[UnityEngine.Random.Range(0, _recipeListSO.RecipeSOs.Count)];
                _waitingRecipeSOs.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < _waitingRecipeSOs.Count; i++)
        {
            RecipeSO waitingRecipeSO = _waitingRecipeSOs[i];

            if (waitingRecipeSO.KitchenObjectSOs.Count == plateKitchenObject.KitchenObjectSOs.Count)
            {
                // Has the same number of ingredients
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.KitchenObjectSOs)
                {
                    // Cycling through all ingredients in the recipe
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.KitchenObjectSOs)
                    {
                        // Cycling through all ingredients in the Plate
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            // Ingredients match!
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        // This recipe ingredient was not found on the Plate
                        plateContentsMatchesRecipe = false;
                    }
                }

                if (plateContentsMatchesRecipe)
                {
                    // Player delivered the correct recipe!
                    _waitingRecipeSOs.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }

        // No matches found
        // Player did not deliver a correct recipe
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }
}
