CREATE FUNCTION ufn_CalculateFutureValue (
	@InitialSum MONEY, 
	@YearlyInterestRate FLOAT, 
	@NumberOfYears INT
	)
RETURNS MONEY
AS
BEGIN
	DECLARE @Result MONEY
	SET @Result = @InitialSum * POWER(1 + @YearlyInterestRate, @NumberOfYears)
	RETURN @Result
END