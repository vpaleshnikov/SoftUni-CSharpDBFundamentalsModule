CREATE VIEW v_UserWithCountries AS
	 SELECT CONCAT(cs.FirstName, ' ', cs.LastName) AS CustomerName,
		    cs.Age,
		    cs.Gender,
		    cn.Name AS CountryName
	   FROM Customers AS cs
 INNER JOIN Countries AS cn
		 ON cn.Id = cs.CountryId