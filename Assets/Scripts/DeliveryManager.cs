using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DeliveryManager : NetworkBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    [SerializeField] private RecipeListSO _recipeListSO;

    private List<RecipeSO> _waitingRecipeSOs;
    private float _spawnRecipeTimer = 4f;
    private float _spawnRecipeTimerMax = 4f;
    private int _waitingRecipesMax = 4;
    private int _successfulRecipesAmount;

    public List<RecipeSO> WaitingRecipeSOs => _waitingRecipeSOs;
    public int SuccessfulRecipesAmount => _successfulRecipesAmount;

    void Awake()
    {
        Instance = this;

        _waitingRecipeSOs = new List<RecipeSO>();
    }

    void Update()
    {
        if (!IsServer)
        {
            return;
        }

        _spawnRecipeTimer -= Time.deltaTime;
        if (_spawnRecipeTimer <= 0)
        {
            _spawnRecipeTimer = _spawnRecipeTimerMax;

            if (GameManager.Instance.IsGamePlaying() && _waitingRecipeSOs.Count < _waitingRecipesMax)
            {
                int waitingRecipeSOIndex = UnityEngine.Random.Range(0, _recipeListSO.RecipeSOs.Count);

                SpawnNewWaitingRecipeClientRpc(waitingRecipeSOIndex);
            }
        }
    }

    [ClientRpc]
    private void SpawnNewWaitingRecipeClientRpc(int waitingRecipeSOIndex)
    {
        RecipeSO waitingRecipeSO = _recipeListSO.RecipeSOs[waitingRecipeSOIndex];

        _waitingRecipeSOs.Add(waitingRecipeSO);

        OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
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
                    DeliverCorrectRecipeServerRpc(i);
                    return;
                }
            }
        }

        // No matches found
        // Player did not deliver a correct recipe
        DeliverIncorrectRecipeServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliverIncorrectRecipeServerRpc()
    {
        DeliverIncorrectRecipeClientRpc();
    }

    [ClientRpc]
    private void DeliverIncorrectRecipeClientRpc()
    {
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliverCorrectRecipeServerRpc(int waitingRecipeSOListIndex)
    {
        DeliverCorrectRecipeClientRpc(waitingRecipeSOListIndex);
    }

    [ClientRpc]
    private void DeliverCorrectRecipeClientRpc(int waitingRecipeSOListIndex)
    {
        _successfulRecipesAmount++;

        _waitingRecipeSOs.RemoveAt(waitingRecipeSOListIndex);

        OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
        OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
    }
}
