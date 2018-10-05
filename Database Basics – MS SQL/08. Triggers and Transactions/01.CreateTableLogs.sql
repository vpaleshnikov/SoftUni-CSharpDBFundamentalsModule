CREATE TRIGGER tr_AccountBalanceChange ON Accounts FOR UPDATE
AS
BEGIN
	DECLARE @AccountId int = (SELECT Id FROM inserted)
	DECLARE @OldBalance money = (SELECT Balance FROM deleted)
	DECLARE @NewBalance money = (SELECT Balance FROM inserted)
	
	IF(@NewBalance <> @OldBalance)
	BEGIN
		INSERT INTO Logs VALUES (@AccountId, @OldBalance, @NewBalance)
	END
END