CREATE PROC usp_GetHoldersWithBalanceHigherThan (@Balance Decimal(10, 2))
AS
BEGIN
	WITH CTE_MinBalanceAccountHolders (HolderId) AS (
    SELECT AccountHolderId FROM Accounts
    GROUP BY AccountHolderId
    HAVING SUM(Balance) > @Balance
	)

  SELECT ah.FirstName AS [First Name], ah.LastName AS [Last Name]
  FROM CTE_MinBalanceAccountHolders AS minBalanceHolders 
  JOIN AccountHolders AS ah ON ah.Id = minBalanceHolders.HolderId
  ORDER BY ah.LastName, ah.FirstName 
END