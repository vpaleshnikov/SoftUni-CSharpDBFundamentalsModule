CREATE FUNCTION ufn_IsWordComprised(@SetOfLetters VARCHAR(50), @Word VARCHAR(50))
RETURNS BIT
AS
BEGIN
	DECLARE @CurrentLetter CHAR;
	DECLARE @Counter INT = 1;

	WHILE (LEN(@Word) >= @Counter)
	BEGIN
		SET @CurrentLetter = SUBSTRING(@Word, @Counter, 1)
		DECLARE @Match INT = CHARINDEX(@CurrentLetter, @SetOfLetters)

		IF(@Match = 0)
		BEGIN
			RETURN 0
		END
		SET @Counter += 1;
	END

	RETURN 1
END