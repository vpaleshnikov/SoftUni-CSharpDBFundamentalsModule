SELECT TOP 3 
           EmployeeID, 
           FirstName 
      FROM Employees
     WHERE EmployeeID 
	NOT IN (SELECT DISTINCT EmployeeID FROM EmployeesProjects)
  ORDER BY EmployeeID