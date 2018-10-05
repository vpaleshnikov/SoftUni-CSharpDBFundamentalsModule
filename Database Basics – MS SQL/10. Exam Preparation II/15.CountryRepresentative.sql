SELECT CountryName, DistributorName
FROM (
	SELECT c.[Name] AS CountryName, d.[Name] AS DistributorName, 
		COUNT(i.Id) AS IngredientCount, 
		DENSE_RANK() OVER(PARTITION BY c.[Name] ORDER BY COUNT(i.Id) DESC) AS [Rank]
	FROM Countries AS c
	JOIN Distributors AS d ON d.CountryId = c.Id
	LEFT JOIN Ingredients AS i ON d.Id = i.DistributorId
	GROUP BY c.[Name], d.[Name]
) AS RankedDistributors
WHERE [Rank] = 1
ORDER BY CountryName, DistributorName