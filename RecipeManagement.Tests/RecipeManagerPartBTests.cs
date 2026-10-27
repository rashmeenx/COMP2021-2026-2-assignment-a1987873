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
}