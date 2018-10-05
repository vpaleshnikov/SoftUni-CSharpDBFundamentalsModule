SELECT TOP 1 WITH TIES
		   cn.Name AS CountryName, 
		   AVG(f.Rate) AS FeedbackRate 
	  FROM Countries AS cn
INNER JOIN Customers AS cs
		ON cs.CountryId = cn.Id
INNER JOIN Feedbacks AS f
		ON f.CustomerId = cs.Id
  GROUP BY cn.Name
  ORDER BY AVG(f.Rate) DESC