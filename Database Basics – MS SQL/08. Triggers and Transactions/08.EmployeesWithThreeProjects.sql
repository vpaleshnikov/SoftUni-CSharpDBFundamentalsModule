CREATE PROC usp_AssignProject(@emloyeeId INT, @projectID INT) 
AS
BEGIN
	BEGIN TRANSACTION
		DECLARE @CountOfProjects INT = (SELECT COUNT(*) 
									   FROM EmployeesProjects
									   WHERE EmployeeID = @emloyeeId)

		INSERT INTO EmployeesProjects
		VALUES
		(@emloyeeId, @projectID)

		IF(@CountOfProjects >= 3)
		BEGIN
			RAISERROR('The employee has too many projects!', 16, 1)
			ROLLBACK
		END
	COMMIT
END