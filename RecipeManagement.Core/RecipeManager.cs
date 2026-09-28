using System;
using System.Collections.Generic;

namespace RecipeManagement.Core;

/// <summary>
/// Implement this class using the five Part A collections as private fields:
/// Dictionary&lt;int, Recipe&gt;, List&lt;string&gt;, LinkedList&lt;int&gt;,
/// Stack&lt;int&gt; and Queue&lt;string&gt;.
/// </summary>
public sealed class RecipeManager : IRecipeManager
{
    // TODO Part A: add your private collection fields here.
    private readonly Dictionary<int, Recipe> _recipes = new();
    private readonly List<string> _shoppingList = new();
    private readonly LinkedList<int> _cookingPlan = new();
    private readonly Stack<int> _removedRecipeHistory = new();
    private readonly Queue<string> _instructionQueue = new();


    public RecipeManager(IEnumerable<Recipe> recipes)
    {
        // TODO Part A: validate recipes and build Dictionary<int, Recipe>.
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
        if (!_cookingPlan.Contains(recipeId))
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
    
    public IReadOnlyList<int> GetCookingPlan() =>
        throw new NotImplementedException("Part A: implement GetCookingPlan.");

    public bool StartCooking(int recipeId) =>
        throw new NotImplementedException("Part A: implement StartCooking.");

    public string? PeekNextInstruction() =>
        throw new NotImplementedException("Part A: implement PeekNextInstruction.");

    public string? CompleteNextInstruction() =>
        throw new NotImplementedException("Part A: implement CompleteNextInstruction.");

    public IReadOnlyList<Recipe> SearchByTitle(string searchText) =>
        throw new NotImplementedException("Part B: implement SearchByTitle.");

    public IReadOnlyList<Recipe> SearchByIngredient(string searchText) =>
        throw new NotImplementedException("Part B: implement SearchByIngredient.");

    public IReadOnlyList<Recipe> GetHighestProteinRecipes(int count) =>
        throw new NotImplementedException("Part B: implement GetHighestProteinRecipes.");

    public bool AddSavedRecipe(int recipeId) =>
        throw new NotImplementedException("Part B: implement AddSavedRecipe.");

    public bool RemoveSavedRecipe(int recipeId) =>
        throw new NotImplementedException("Part B: implement RemoveSavedRecipe.");

    public bool IsRecipeSaved(int recipeId) =>
        throw new NotImplementedException("Part B: implement IsRecipeSaved.");

    public IReadOnlyList<int> GetSavedRecipes() =>
        throw new NotImplementedException("Part B: implement GetSavedRecipes.");
}
