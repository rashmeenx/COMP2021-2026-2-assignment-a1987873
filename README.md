# Recipe Management System — Student Starter

Recipe management application developed for COMP2021 Software Development Practice.

The project contains the Part A recipe-management features and the Part B extensions for LINQ searching, protein checking and saved recipes.

## Project Structure
### Supplied

- `RecipeManagement.Core/Models/` — recipe model classes
- `RecipeManagement.Core/RecipeLoader.cs` — reads `data/recipes.json`
- `RecipeManagement.Core/IRecipeManager.cs` — public API
- `RecipeManagement.Application/` — console menu (options labelled PartA / PartB)
- `RecipeManagement.Tests/` — example tests
- `data/recipes.json` — recipe dataset

## What I implemented
**Part A**
The Part A implementation uses the following C# collections in `RecipeManager.cs`:

- `Dictionary<int, Recipe>`
- `List<string>`
- `LinkedList<int>`
- `Stack<int>`
- `Queue<string>`

**Part B** 
Part B extends the existing RecipeManager with:

- case-insensitive partial recipe title search using LINQ
- case-insensitive ingredient search using LINQ
- a highest-protein recipe report using LINQ
- saved/favourite recipes using HashSet<int>
- additional xUnit tests for the new Part B behaviour
- Design.md explaining the design choice


## Build and run

Open `RecipeManagement.sln`:

```bash
dotnet build
dotnet test
dotnet run --project RecipeManagement.Application -- data/recipes.json
```

Until you implement `RecipeManager`, menu options print a **Not implemented** message.

## AI acknowledgement

I did not use generative AI in the preparation of this assignment.

# COMP2021-2026-2-assignment-a1987873
#### .gitignore file reference: https://gist.github.com/kmorcinek/2710267#file-gitignore

