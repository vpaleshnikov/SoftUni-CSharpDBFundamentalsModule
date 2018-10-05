       USE SoftUni
        GO
SELECT TOP 50
           e.FirstName, 
           e.LastName, 
           t.Name AS Town, 
           a.AddressText 
      FROM Employees AS e
INNER JOIN Addresses AS a
        ON a.AddressID = e.AddressID
INNER JOIN Towns as t
        ON t.TownId = a.TownID
  ORDER BY FirstName, LastName