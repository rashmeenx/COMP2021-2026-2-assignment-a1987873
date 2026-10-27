using System.Collections.Generic;
using RecipeManagement.Core;
using Xunit;

namespace RecipeManagement.Tests;


public sealed class RecipeManagerPartBTests
{
    private static RecipeManager CreateManager()
    {
        return new RecipeManager(new[]
        {
            new Recipe
            {
                Id = 10,
                Title = "Creamy Pasta",
                Ingredients = new()
                {
                    "Pasta",
                    "Tomato sauce",
                    "Tomato paste"
                },
                Instructions = new()
                {
                    "Boil pasta",
                    "Add sauce"
                },
                Nutrition = new NutritionInfo
                {
                    ProteinG = 20
                }
            },

            new Recipe
            {
                Id = 20,
                Title = "Tomato Soup",
                Ingredients = new()
                {
                    "Fresh tomato",
                    "Water"
                },
                Instructions = new()
                {
                    "Cook tomato",
                    "Blend soup"
                },
                Nutrition = new NutritionInfo
                {
                    ProteinG = 10
                }
            },

            new Recipe
            {
                Id = 30,
                Title = "Chicken Salad",
                Ingredients = new()
                {
                    "Chicken",
                    "Lettuce"
                },
                Instructions = new()
                {
                    "Cook chicken",
                    "Mix salad"
                },
                Nutrition = new NutritionInfo
                {
                    ProteinG = 35
                }
            },

            new Recipe
            {
                Id = 40,
                Title = "Fruit Bowl",
                Ingredients = new()
                {
                    "Apple",
                    "Banana"
                },
                Instructions = new()
                {
                    "Cut fruit"
                },
                Nutrition = null
            }
        });
    }

    
    [Fact]
    public void SearchByTitle_FindsPartialMatchIgnoringCase()
    {
        var manager = CreateManager();

        IReadOnlyList<Recipe> results =
            manager.SearchByTitle("pAsTa");

        Assert.Single(results);
        Assert.Equal(10, results[0].Id);
        Assert.Equal("Creamy Pasta", results[0].Title);
    }

    [Fact]
    public void SearchByIngredient_FindsMatchesIgnoringCase()
    {
        var manager = CreateManager();

        IReadOnlyList<Recipe> results =
            manager.SearchByIngredient("toMaTo");

        Assert.Equal(2, results.Count);

        Assert.Contains(
            results,
            recipe => recipe.Id == 10);

        Assert.Contains(
            results,
            recipe => recipe.Id == 20);
    }

    [Fact]
    public void SearchByIngredient_ReturnsRecipeOnlyOnce()
    {
        var manager = CreateManager();

        IReadOnlyList<Recipe> results =
            manager.SearchByIngredient("tomato");

        int pastaCount = 0;

        foreach (Recipe recipe in results)
        {
            if (recipe.Id == 10)
            {
                pastaCount++;
            }
        }

        Assert.Equal(1, pastaCount);
    }

    [Fact]
    public void HighestProteinRecipes_AreReturnedInDescendingOrder()
    {
        var manager = CreateManager();

        IReadOnlyList<Recipe> results =
            manager.GetHighestProteinRecipes(2);

        Assert.Equal(2, results.Count);

        Assert.Equal(
            30,
            results[0].Id);

        Assert.Equal(
            10,
            results[1].Id);
    }

    [Fact]
    public void HighestProteinRecipes_IgnoreMissingNutrition()
    {
        var manager = CreateManager();

        IReadOnlyList<Recipe> results =
            manager.GetHighestProteinRecipes(10);

        Assert.DoesNotContain(
            results,
            recipe => recipe.Id == 40);
    }

    public void HighestProteinRecipes_ReturnsEmptyForZeroOrNegativeCount()
    {
        var manager = CreateManager();

        Assert.Empty(
            manager.GetHighestProteinRecipes(0));

        Assert.Empty(
            manager.GetHighestProteinRecipes(-1));
    }


}