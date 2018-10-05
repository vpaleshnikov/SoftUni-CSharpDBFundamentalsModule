	SELECT f.ProductId, 
		   CONCAT(c.FirstName, ' ', c.LastName) AS CustomerName,
		   f.Description AS FeedbackDescription
	  FROM Feedbacks AS f
INNER JOIN Customers AS c
		ON c.Id = f.CustomerId
	 WHERE CONCAT(c.FirstName, ' ', c.LastName) IN 
		   (
		   SELECT CONCAT(c.FirstName, ' ', c.LastName) 
		   FROM Feedbacks AS f
		   INNER JOIN Customers AS c
           ON c.Id = f.CustomerId
           GROUP BY CONCAT(c.FirstName, ' ', c.LastName)
           HAVING COUNT(CustomerId) > 2
		   )
  ORDER BY f.ProductId, CustomerName, f.Id