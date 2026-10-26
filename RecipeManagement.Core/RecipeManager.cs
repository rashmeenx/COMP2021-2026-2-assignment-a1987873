using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeManagement.Core;

/// <summary>
/// Manages the recipe catalogue, shopping list, cooking plan, removed-recipe history and cooking instruction queue.
/// </summary>
public sealed class RecipeManager : IRecipeManager
{
    // Private collection fields.
    private readonly Dictionary<int, Recipe> _recipes = new();
    private readonly List<string> _shoppingList = new();
    private readonly LinkedList<int> _cookingPlan = new();
    private readonly Stack<int> _removedRecipeHistory = new();
    private readonly Queue<string> _instructionQueue = new();

    // Stores unique IDs of saved recipes.
    private readonly HashSet<int> _savedRecipes = new();

    public RecipeManager(IEnumerable<Recipe> recipes)
    {
        // Validated recipes and build Dictionary<int, Recipe>.
        ArgumentNullException.ThrowIfNull(recipes);

        foreach (Recipe recipe in recipes)
        {
            if (recipe is null ||
                recipe.Id <= 0 ||
                string.IsNullOrWhiteSpace(recipe.Title) ||
                _recipes.ContainsKey(recipe.Id))
            {
                throw new ArgumentException(
                    "Every recipe must have a positive unique ID and a non-blank title.",
                    nameof(recipes));
            }

            _recipes.Add(recipe.Id, recipe);
        }
    }

    // Changed the Count properties, so that they return the current collection counts.
    public int RecipeCount => _recipes.Count;
    public int ShoppingItemCount => _shoppingList.Count;
    public int CookingPlanCount => _cookingPlan.Count;
    public int PendingInstructionCount => _instructionQueue.Count;
    public int RemovedRecipeCount => _removedRecipeHistory.Count;

    // Implemented adding a valid recipe to the dictionary.
    public bool AddRecipe(Recipe recipe)
    {
        ArgumentNullException.ThrowIfNull(recipe);

        if (recipe.Id <= 0 ||
            string.IsNullOrWhiteSpace(recipe.Title) ||
            _recipes.ContainsKey(recipe.Id))
        {
            return false;
        }

        _recipes.Add(recipe.Id, recipe);

        return true;
    }

    // Implemented recipe lookup using the dictionary.
    public Recipe? FindRecipe(int recipeId)
    {
        if (_recipes.TryGetValue(recipeId, out Recipe? recipe))
        {
            return recipe;
        }

        return null;
    }

    // Implemented recipe removal while preventing removal of planned recipes.
    public bool RemoveRecipe(int recipeId)
    {
        if (!_recipes.ContainsKey(recipeId))
        {
            return false;
        }

        if (_cookingPlan.Contains(recipeId))
        {
            return false;
        }

        return _recipes.Remove(recipeId);
    }

    // Implemented copying a recipe's ingredients into the shopping list.
    public int AddIngredientsToShoppingList(int recipeId)
    {
        if (!_recipes.TryGetValue(recipeId, out Recipe? recipe))
        {
            return 0;
        }

        foreach (string ingredient in recipe.Ingredients)
        {
            _shoppingList.Add(ingredient);
        }

        return recipe.Ingredients.Count;
    }

    // Return a copy so the internal shopping list is not exposed directly.
    public IReadOnlyList<string> GetShoppingList()
    {
        return new List<string>(_shoppingList);
    }

    // Implemented clearing all shopping-list items.
    public void ClearShoppingList()
    {
        _shoppingList.Clear();
    }

    // Implemented adding an existing recipe to the end of the cooking plan.
    public bool AddRecipeToCookingPlan(int recipeId)
    {
        if (!_recipes.ContainsKey(recipeId))
        {
            return false;
        }

        if (_cookingPlan.Contains(recipeId))
        {
            return false;
        }

        _cookingPlan.AddLast(recipeId);

        return true;
    }

    // Implemented cooking-plan removal and storing the removed ID in the stack.
    public bool RemoveRecipeFromCookingPlan(int recipeId)
    {
        LinkedListNode<int>? node = _cookingPlan.Find(recipeId);

        if (node is null)
        {
            return false;
        }

        _cookingPlan.Remove(node);
        _removedRecipeHistory.Push(recipeId);

        return true;
    }

    // Implemented restoring the most recently removed recipe.
    public bool RestoreLastRemovedRecipe()
    {
        if (_removedRecipeHistory.Count == 0)
        {
            return false;
        }

        int recipeId = _removedRecipeHistory.Pop();

        if (!_recipes.ContainsKey(recipeId))
        {
            return false;
        }

        if (_cookingPlan.Contains(recipeId))
        {
            return false;
        }

        _cookingPlan.AddLast(recipeId);

        return true;
    }

    // Implemented stack peek with safe empty-stack handling.
    public int? PeekLastRemovedRecipe()
    {
        if (_removedRecipeHistory.Count == 0)
        {
            return null;
        }

        return _removedRecipeHistory.Peek();
    }

    // Return a copy of the cooking plan in linked-list order.
    public IReadOnlyList<int> GetCookingPlan()
    {
        return new List<int>(_cookingPlan);
    }

    // Implemented loading the selected recipe's instructions into the queue.
    public bool StartCooking(int recipeId)
    {
        if (!_recipes.TryGetValue(recipeId, out Recipe? recipe))
        {
            return false;
        }

        if (recipe.Instructions.Count == 0)
        {
            return false;
        }

        _instructionQueue.Clear();

        foreach (string instruction in recipe.Instructions)
        {
            _instructionQueue.Enqueue(instruction);
        }

        return true;
    }

    // Implemented queue peek without removing the instruction.
    public string? PeekNextInstruction()
    {
        if (_instructionQueue.Count == 0)
        {
            return null;
        }

        return _instructionQueue.Peek();
    }

    // Implemented removing and returning the next queued instruction.
    public string? CompleteNextInstruction()
    {
        if (_instructionQueue.Count == 0)
        {
            return null;
        }

        return _instructionQueue.Dequeue();
    }

    // Part B.
    //  Searches recipe titles for a partial match using LINQ.
    public IReadOnlyList<Recipe> SearchByTitle(string searchText)
    {
        string search = searchText?.Trim() ?? string.Empty;

        return _recipes.Values
            .Where(recipe => recipe.Title.ToLower().Contains(search.ToLower())).ToList();
    }

    // Searches recipe case-insensitive ingredients using LINQ.
    public IReadOnlyList<Recipe> SearchByIngredient(string searchText)
    {
        string search = searchText?.Trim() ?? string.Empty;

        return _recipes.Values
            .Where(recipe => recipe.Ingredients.Any(ingredient => ingredient.ToLower().Contains(search.ToLower()))).ToList();
    }

    // Returns the requested number of recipes sorted by protein content from highest to lowest
    public IReadOnlyList<Recipe> GetHighestProteinRecipes(int count)
    {
        if (count <= 0)
        {
            return new List<Recipe>();
        }

        return _recipes.Values
            .Where(recipe =>
                recipe.Nutrition?.ProteinG.HasValue == true)
            .OrderByDescending(recipe =>
                recipe.Nutrition!.ProteinG!.Value)
            .Take(count)
            .ToList();
    }

    // Adds a recipe to saved recipes if it exists and is not already saved
    public bool AddSavedRecipe(int recipeId)
    {
        if (!_recipes.ContainsKey(recipeId))
        {
            return false;
        }

        return _savedRecipes.Add(recipeId);
    }

    // Removes a recipe from saved recipes
    public bool RemoveSavedRecipe(int recipeId)
    {
        return _savedRecipes.Remove(recipeId);
    }

    // Checks whether a recipe is already saved
    public bool IsRecipeSaved(int recipeId)
    {
        return _savedRecipes.Contains(recipeId);
    }


    // Returns a list of all saved recipe IDs
    public IReadOnlyList<int> GetSavedRecipes()
    {
        return _savedRecipes.ToList();
    }
}
