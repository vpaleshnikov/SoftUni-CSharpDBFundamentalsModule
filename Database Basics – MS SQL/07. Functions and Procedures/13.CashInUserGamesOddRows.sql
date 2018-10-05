CREATE FUNCTION ufn_CashInUsersGames (@GameName NVARCHAR(50))
RETURNS TABLE
AS
RETURN SELECT SUM(Cash) AS SumCash FROM (
	SELECT ug.Cash, ROW_NUMBER() OVER(ORDER BY Cash DESC) AS RowNum
	FROM UsersGames AS ug
	INNER JOIN Games AS g
	ON g.Id = ug.GameId
	WHERE g.Name = @GameName) AS CashList
WHERE RowNum % 2 = 1
