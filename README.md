# COMP2021-2026-2-assignment-a1987873
#### .gitignore file reference: https://gist.github.com/kmorcinek/2710267#file-gitignore

	
	If the same recipe is stored under two different IDs, .Where() can return it twice. But, since there is unique IDs for each recipes in the dictionary, the same recipes will return once. We could use .Distinct() but did not use it as each recipe is stored only once in the dictionary. .Distinct() is used to remove duplicates.
