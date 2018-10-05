USE SoftUni
GO
SELECT TOP(10)  e.FirstName, e.LastName, e.DepartmentID FROM Employees AS e
JOIN (
	SELECT e.DepartmentID, avg(e.Salary) AS avgs FROM Employees AS e
	GROUP BY e.DepartmentID
) AS avgSalaries
ON e.DepartmentID = avgSalaries.DepartmentID
WHERE e.Salary > avgSalaries.avgs
ORDER BY e.DepartmentID