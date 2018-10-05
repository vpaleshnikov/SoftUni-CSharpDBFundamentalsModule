CREATE PROC usp_TransferMoney(@SenderId INT, @ReceiverId INT, @Amount DECIMAL(15,4))
AS
BEGIN
	BEGIN TRAN
	EXEC dbo.usp_WithdrawMoney @SenderId, @Amount
	EXEC dbo.usp_DepositMoney @ReceiverId, @Amount

	DECLARE @SenderBalance DECIMAL;
	SET @SenderBalance = (SELECT Balance FROM Accounts WHERE Id = @SenderId)

	IF (@SenderBalance < 0)
	BEGIN
		ROLLBACK
		RETURN
	END
	COMMIT
END