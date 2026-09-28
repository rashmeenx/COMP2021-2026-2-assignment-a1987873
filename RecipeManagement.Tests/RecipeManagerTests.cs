using System.Collections.Generic;
using RecipeManagement.Core;

namespace RecipeManagement.Tests;

/// <summary>
/// Example tests from the assignment specification. Add your own tests as you work.
/// </summary>
public sealed class RecipeManagerTests
{
    // ---------------------------------------------------------
    // Supplied example tests
    // ---------------------------------------------------------

    [Fact]
    public void Constructor_BuildsRecipeDictionary()
    {
        var manager = CreateManager();

        Assert.Equal(2, manager.RecipeCount);
        Assert.Equal("Recipe A", manager.FindRecipe(10)?.Title);
    }

    [Fact]
    public void InstructionsAreCompletedInFileOrder()
    {
        var manager = CreateManager();

        Assert.True(manager.StartCooking(10));
        Assert.Equal("First step", manager.PeekNextInstruction());
        Assert.Equal("First step", manager.CompleteNextInstruction());
        Assert.Equal("Second step", manager.PeekNextInstruction());
    }

    [Fact]
    public void RemovedRecipesAreRestoredLastInFirstOut()
    {
        var manager = CreateManager();

        manager.AddRecipeToCookingPlan(10);
        manager.AddRecipeToCookingPlan(20);

        manager.RemoveRecipeFromCookingPlan(10);
        manager.RemoveRecipeFromCookingPlan(20);

        Assert.Equal(20, manager.PeekLastRemovedRecipe());
        Assert.True(manager.RestoreLastRemovedRecipe());
        Assert.Equal(new[] { 20 }, manager.GetCookingPlan());
    }


    // ---------------------------------------------------------
    // Student Part A tests
    // ---------------------------------------------------------

    [Fact]
    public void AddRecipe_AddsValidRecipe()
    {
        var manager = CreateManager();

        var recipe = new Recipe
        {
            Id = 30,
            Title = "Recipe C"
        };

        bool added = manager.AddRecipe(recipe);

        Assert.True(added);
        Assert.Equal(3, manager.RecipeCount);
        Assert.Equal("Recipe C", manager.FindRecipe(30)?.Title);
    }

    [Fact]
    public void AddRecipe_RejectsDuplicateRecipeId()
    {
        var manager = CreateManager();

        var duplicateRecipe = new Recipe
        {
            Id = 10,
            Title = "Duplicate Recipe"
        };

        bool added = manager.AddRecipe(duplicateRecipe);

        Assert.False(added);
        Assert.Equal(2, manager.RecipeCount);
    }

    [Fact]
    public void FindRecipe_ReturnsNullForMissingRecipe()
    {
        var manager = CreateManager();

        Recipe? recipe = manager.FindRecipe(999);

        Assert.Null(recipe);
    }

    [Fact]
    public void RemoveRecipe_RemovesExistingRecipe()
    {
        var manager = CreateManager();

        bool removed = manager.RemoveRecipe(10);

        Assert.True(removed);
        Assert.Equal(1, manager.RecipeCount);
        Assert.Null(manager.FindRecipe(10));
    }

    [Fact]
    public void RemoveRecipe_FailsWhenRecipeIsInCookingPlan()
    {
        var manager = CreateManager();

        manager.AddRecipeToCookingPlan(10);

        bool removed = manager.RemoveRecipe(10);

        Assert.False(removed);
        Assert.NotNull(manager.FindRecipe(10));
    }

    [Fact]
    public void ShoppingList_AddsIngredientsInOrder()
    {
        var manager = CreateManager();

        int added = manager.AddIngredientsToShoppingList(10);

        Assert.Equal(1, added);
        Assert.Equal(1, manager.ShoppingItemCount);

        Assert.Equal(
            new[] { "1 apple" },
            manager.GetShoppingList());
    }

    [Fact]
    public void ShoppingList_ReturnsZeroForMissingRecipe()
    {
        var manager = CreateManager();

        int added = manager.AddIngredientsToShoppingList(999);

        Assert.Equal(0, added);
        Assert.Equal(0, manager.ShoppingItemCount);
    }

    [Fact]
    public void ClearShoppingList_RemovesAllItems()
    {
        var manager = CreateManager();

        manager.AddIngredientsToShoppingList(10);

        manager.ClearShoppingList();

        Assert.Equal(0, manager.ShoppingItemCount);
        Assert.Empty(manager.GetShoppingList());
    }

    [Fact]
    public void CookingPlan_AddsRecipesInOrder()
    {
        var manager = CreateManager();

        Assert.True(manager.AddRecipeToCookingPlan(10));
        Assert.True(manager.AddRecipeToCookingPlan(20));

        Assert.Equal(
            new[] { 10, 20 },
            manager.GetCookingPlan());
    }

    [Fact]
    public void CookingPlan_RejectsDuplicateRecipe()
    {
        var manager = CreateManager();

        bool firstAdd = manager.AddRecipeToCookingPlan(10);
        bool secondAdd = manager.AddRecipeToCookingPlan(10);

        Assert.True(firstAdd);
        Assert.False(secondAdd);
        Assert.Equal(1, manager.CookingPlanCount);
    }

    [Fact]
    public void CookingPlan_RejectsMissingRecipe()
    {
        var manager = CreateManager();

        bool added = manager.AddRecipeToCookingPlan(999);

        Assert.False(added);
        Assert.Equal(0, manager.CookingPlanCount);
    }

    [Fact]
    public void RemovingRecipeFromCookingPlan_AddsItToHistory()
    {
        var manager = CreateManager();

        manager.AddRecipeToCookingPlan(10);

        bool removed = manager.RemoveRecipeFromCookingPlan(10);

        Assert.True(removed);
        Assert.Equal(0, manager.CookingPlanCount);
        Assert.Equal(1, manager.RemovedRecipeCount);
        Assert.Equal(10, manager.PeekLastRemovedRecipe());
    }

    [Fact]
    public void EmptyRemovedRecipeHistory_IsHandledSafely()
    {
        var manager = CreateManager();

        Assert.Null(manager.PeekLastRemovedRecipe());
        Assert.False(manager.RestoreLastRemovedRecipe());
    }

    [Fact]
    public void EmptyInstructionQueue_IsHandledSafely()
    {
        var manager = CreateManager();

        Assert.Null(manager.PeekNextInstruction());
        Assert.Null(manager.CompleteNextInstruction());
    }

    [Fact]
    public void StartCooking_ReturnsFalseForMissingRecipe()
    {
        var manager = CreateManager();

        bool started = manager.StartCooking(999);

        Assert.False(started);
        Assert.Equal(0, manager.PendingInstructionCount);
    }

    [Fact]
    public void CookingPlanAndHistory_WorkTogether()
    {
        var manager = CreateManager();

        Assert.True(manager.AddRecipeToCookingPlan(10));

        Assert.True(manager.RemoveRecipeFromCookingPlan(10));

        Assert.Equal(10, manager.PeekLastRemovedRecipe());

        Assert.True(manager.RestoreLastRemovedRecipe());

        Assert.Equal(
            new[] { 10 },
            manager.GetCookingPlan());
    }


    // ---------------------------------------------------------
    // Supplied helper method
    // ---------------------------------------------------------

    private static RecipeManager CreateManager()
    {
        return new RecipeManager(new[]
        {
            new Recipe
            {
                Id = 10,
                Title = "Recipe A",
                Ingredients = new() { "1 apple" },
                Instructions = new() { "First step", "Second step" }
            },
            new Recipe
            {
                Id = 20,
                Title = "Recipe B"
            }
        });
    }
}