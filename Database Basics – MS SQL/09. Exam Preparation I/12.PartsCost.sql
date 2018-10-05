    SELECT ISNULL(SUM(p.Price * op.Quantity), 0) AS [Parts Total] 
      FROM Parts AS p
INNER JOIN OrderParts AS op
        ON op.PartId = p.PartId
INNER JOIN Orders AS o
        ON o.OrderId = op.OrderId
     WHERE DATEDIFF(WEEK, o.IssueDate, '2017-04-24') <= 3