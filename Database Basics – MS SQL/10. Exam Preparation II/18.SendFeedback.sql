CREATE PROC usp_SendFeedback @customerId INT, @productId INT, @rate DECIMAL(4, 2),
	@description NVARCHAR(255) AS
BEGIN
BEGIN TRANSACTION
	INSERT INTO Feedbacks (CustomerId, ProductId, Rate, [Description]) VALUES
	(@customerId, @productId, @rate, @description)
	DECLARE @feedbackCount INT = (
		SELECT COUNT(f.Id)
		FROM Feedbacks AS f
		WHERE ProductId = @productId AND CustomerId = @customerId)
	IF @feedbackCount > 3
	BEGIN
		ROLLBACK
		RAISERROR('You are limited to only 3 feedbacks per product!', 16, 1)		
	END
	ELSE
		COMMIT
END