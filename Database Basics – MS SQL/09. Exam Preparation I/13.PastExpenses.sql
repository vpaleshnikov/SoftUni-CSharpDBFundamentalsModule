  SELECT j.JobId,
	     (SELECT ISNULL(SUM(p.Price * op.Quantity), 0)
	     FROM Parts AS p
	     INNER JOIN OrderParts AS op
	     ON op.PartId = p.PartId
	     INNER JOIN Orders AS o
	     ON o.OrderId = op.OrderId
	     INNER JOIN Jobs AS jo
	     ON jo.JobId = o.JobId
	     WHERE jo.JobId = j.JobId) AS Total
    FROM Jobs AS j
   WHERE j.Status = 'Finished'
ORDER BY Total DESC, j.JobId