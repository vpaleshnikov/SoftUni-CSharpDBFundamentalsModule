CREATE FUNCTION udf_GetCost (@JobId INT)
RETURNS DECIMAL(15, 2)
AS
BEGIN
	DECLARE @Result DECIMAL(15, 2)
	SET @Result =
	(SELECT ISNULL(SUM(p.Price), 0) FROM Orders AS o
	INNER JOIN OrderParts AS op
	ON op.OrderId = o.OrderId
	INNER JOIN Parts AS p
	ON p.PartId = op.PartId
	WHERE o.JobId = @JobId)
	RETURN @Result
END