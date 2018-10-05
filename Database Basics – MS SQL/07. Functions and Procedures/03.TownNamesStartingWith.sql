CREATE PROC usp_GetTownsStartingWith @InputString NVARCHAR(MAX)
AS
BEGIN
	SELECT Name AS Town FROM Towns
	WHERE Name LIKE @InputString + '%'
END