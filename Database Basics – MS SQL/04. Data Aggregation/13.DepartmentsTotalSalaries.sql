USE SoftUni
GO
SELECT DepartmentId, SUM(Salary) AS TotalSalary FROM Employees
GROUP BY DepartmentID