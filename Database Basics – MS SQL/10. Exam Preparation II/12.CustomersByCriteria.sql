    SELECT cs.FirstName, 
		   cs.Age, 
		   cs.PhoneNumber 
	  FROM Customers AS cs
INNER JOIN Countries AS cn
		ON cn.Id = cs.CountryId
	 WHERE (cs.Age >= 21 AND FirstName LIKE '%an%') OR 
		   (RIGHT(cs.PhoneNumber, 2) = 38 AND cn.Name <> 'Greece')
  ORDER BY cs.FirstName, cs.Age DESC