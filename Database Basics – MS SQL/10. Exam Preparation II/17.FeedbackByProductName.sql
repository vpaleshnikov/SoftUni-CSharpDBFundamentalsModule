CREATE FUNCTION udf_GetRating (@Name NVARCHAR(30))
RETURNS VARCHAR(10) 
AS
BEGIN
	DECLARE @AvgRate DECIMAL(4, 2) = (SELECT AVG(Rate) 
									 FROM Products AS p
									 JOIN Feedbacks AS f ON p.Id = f.ProductId
									 WHERE [Name] = @Name)
	DECLARE @Rating VARCHAR(10) =
		CASE 
		WHEN @AvgRate IS NULL THEN 'No rating'
		WHEN @AvgRate < 5.00 THEN 'Bad'
		WHEN @AvgRate <= 8.00 THEN 'Average'
		ELSE 'Good' 
		END
	RETURN @Rating
END