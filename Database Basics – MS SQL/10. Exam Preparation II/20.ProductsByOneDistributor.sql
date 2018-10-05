WITH CTE AS (
	SELECT p.Id AS ProductId,
		p.[Name] AS ProductName, 
		AVG(f.Rate) AS AverageRate,
		d.[Name] AS DistributorName,
		c.[Name] AS DistributorCountry 
	FROM Products AS p
	JOIN Feedbacks AS f ON p.Id = f.ProductId
	JOIN ProductsIngredients AS pii ON p.Id = pii.ProductId
	JOIN Ingredients AS i ON pii.IngredientId = i.Id
	JOIN Distributors AS d ON i.DistributorId = d.Id
	JOIN Countries AS c ON c.Id = d.CountryId
	GROUP BY p.[Name], d.[Name], c.[Name], p.Id)
SELECT CTE.ProductName, AverageRate, DistributorName, DistributorCountry
FROM CTE
JOIN (
	SELECT ProductName, COUNT(DistributorName) AS DistributorCount
	FROM CTE
	GROUP BY ProductName
) AS DistributorCount ON CTE.ProductName = DistributorCount.ProductName
WHERE DistributorCount = 1
ORDER BY ProductId