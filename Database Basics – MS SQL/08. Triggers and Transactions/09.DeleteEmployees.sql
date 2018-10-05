CREATE TRIGGER tr_SaveDeletedEmployees ON Employees FOR DELETE
AS
BEGIN
	INSERT INTO Deleted_Employees
		 SELECT 
				d.FirstName, 
				d.LastName, 
				d.MiddleName,
				d.JobTitle,
				d.DepartmentID,
				d.Salary
		   FROM deleted AS d
END