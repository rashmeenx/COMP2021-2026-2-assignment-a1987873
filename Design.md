# Design

**Part B**.

## 1. Saved recipe collection

I selected HashSet<int> for the saved/favourite recipe feature because it efficiently supports adding, removing, duplicate prevention, and membership checks. It stores recipe IDs and does not duplicate the existing Recipe objects.

Add: HashSet.Add() efficiently inserts a recipe ID in O(1) average time. The AddSavedRecipe method also checks that the recipe exists in the original catalogue before saving it.

Remove: HashSet.Remove() efficiently removes a saved recipe ID in O(1) average time and returns false if the ID is not present.

Duplicate prevention: A HashSet only stores unique values, so the same recipe ID cannot be saved twice. The Add() method automatically returns false when attempting to add an existing ID.

Membership checks: HashSet.Contains() efficiently checks whether a recipe ID is already saved in O(1) average time without needing to go through the entire collection. This is used in IsRecipeSaved().

Finally, GetSavedRecipes() converts the HashSet<int> into a list using .ToList() to return the saved recipe IDs.

Overall, HashSet<int> is suitable because it automatically prevents duplicates and provides add, remove, and membership-check operations.

## 2. Integration

The Part B features extend the existing Part A RecipeManager without replacing its existing collections or behaviours. The existing Dictionary<int, Recipe> remains the main recipe catalogue. SearchByTitle, SearchByIngredient and GetHighestProteinRecipes use LINQ over _recipes.Values  to access the existing recipe objects, filter them based on title, ingredient or protein content, and sort the protein results where required. The saved-recipe feature introduces a new HashSet<int> to store unique recipe IDs without duplicating the original Recipe objects. AddSavedRecipe() uses _recipes.ContainsKey(recipeId) to check whether a recipe exists in the Part A catalogue before saving its ID. This allows all Part B features to reuse the existing recipe data while preserving the Part A functionality.

## 3. Basic complexity

| Operation | Structure | Expected complexity | Reason |
| --- | --- | --- | --- |
| Lookup recipe by ID | Dictionary | Average O(1) | Hash-based key lookup. |
| Traverse cooking plan | LinkedList | O(n) | Each planned recipe may need to be visited. |
| Complete next instruction | Queue | O(1) | The item at the front is removed. |
| LINQ title/ingredient search | Recipe collection | O(n) | Each recipe may need to be inspected. |
| Check whether a recipe is saved | HashSet<int> | Average O(1) | HashSet.Contains uses hash-based membership lookup to check whether the ID exists and prevent duplication.|
