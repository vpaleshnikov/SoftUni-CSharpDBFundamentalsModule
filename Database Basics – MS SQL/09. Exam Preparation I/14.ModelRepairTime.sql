    SELECT j.ModelId, 
		   m.Name,
		   CAST(AVG(DATEDIFF(DAY, j.IssueDate, j.FinishDate)) AS VARCHAR) + ' days' AS [Average Service Time]
      FROM Models AS m
INNER JOIN Jobs AS j
        ON j.ModelId = m.ModelId
  GROUP BY j.ModelId, m.Name
  ORDER BY AVG(DATEDIFF(DAY, j.IssueDate, j.FinishDate))