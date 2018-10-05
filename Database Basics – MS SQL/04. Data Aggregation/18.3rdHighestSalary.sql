USE SoftUni
GO
SELECT DepartmentId, Salary FROM (
	SELECT 
		DepartmentId, 
		Salary,
		DENSE_RANK() OVER (PARTITION BY DepartmentId ORDER BY Salary DESC) AS Rank
	FROM Employees
	GROUP BY DepartmentID, Salary
) AS RankedSalaries
WHERE Rank = 3