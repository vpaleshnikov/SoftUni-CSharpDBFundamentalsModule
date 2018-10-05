SELECT TOP 1 WITH TIES
	       m.Name, 
	       COUNT(*) AS [Times Serviced],
		   (
		   SELECT ISNULL(SUM(p.Price * op.Quantity), 0) FROM Jobs AS j
		   INNER JOIN Orders AS o
		   ON o.JobId = j.JobId
           INNER JOIN OrderParts AS op
		   ON op.OrderId = o.OrderId
           INNER JOIN Parts AS p
		   ON p.PartId = op.PartId
		   WHERE j.ModelId = m.ModelId
		   ) 
		   AS [Parts Total]
	  FROM Models AS m
INNER JOIN Jobs AS j
        ON j.ModelId = m.ModelId
  GROUP BY m.Name, m.ModelId
  ORDER BY [Times Serviced] DESC