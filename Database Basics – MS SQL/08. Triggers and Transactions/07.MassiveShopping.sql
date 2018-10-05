BEGIN TRANSACTION
DECLARE @Sum DECIMAL = (SELECT SUM(i.Price) 
					   FROM Items AS i
					   WHERE MinLevel BETWEEN 11 AND 12)

IF ((SELECT Cash FROM UsersGames WHERE Id = 110) < @Sum)
BEGIN
	ROLLBACK
END
ELSE 
BEGIN
	UPDATE UsersGames
	SET Cash -= @Sum
	WHERE Id = 110

	INSERT INTO UserGameItems (UserGameId, ItemId)
	SELECT 110, Id 
	FROM Items 
	WHERE MinLevel BETWEEN 11 AND 12
	COMMIT
END

BEGIN TRANSACTION
DECLARE @Sum2 DECIMAL = (SELECT SUM(i.Price)
						FROM Items i
						WHERE MinLevel BETWEEN 19 AND 21)

IF (SELECT Cash FROM UsersGames WHERE Id = 110) < @Sum2
BEGIN
	ROLLBACK
END
ELSE 
BEGIN
	UPDATE UsersGames
	SET Cash = Cash - @sum2
	WHERE Id = 110

	INSERT INTO UserGameItems (UserGameId, ItemId)
	SELECT 110, Id 
	FROM Items 
	WHERE MinLevel BETWEEN 19 AND 21
	COMMIT
END

SELECT i.Name AS [Item Name] 
  FROM UserGameItems ugi
  JOIN Items i 
    ON ugi.ItemId = i.Id
 WHERE ugi.UserGameId = 110