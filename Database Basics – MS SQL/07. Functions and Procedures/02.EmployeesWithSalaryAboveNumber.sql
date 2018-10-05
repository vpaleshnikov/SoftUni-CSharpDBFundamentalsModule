CREATE PROC usp_GetEmployeesSalaryAboveNumber @InputSalary DECIMAL(18, 4)
AS
BEGIN
	SELECT FirstName, LastName FROM Employees
	WHERE Salary >= @InputSalary
END