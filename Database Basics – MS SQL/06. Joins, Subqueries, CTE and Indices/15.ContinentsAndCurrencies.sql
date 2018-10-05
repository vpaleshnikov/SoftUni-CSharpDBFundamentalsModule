SELECT sel.ContinentCode,
       sel.CurrencyCode AS CurrencyCode,
       sel.CurrencyUs AS CurrencyUsage
  FROM (
	                SELECT c.ContinentCode,
                    cr.CurrencyCode,
                    COUNT(cr.Description) AS CurrencyUs,
                    DENSE_RANK() OVER 
					(
					PARTITION BY c.ContinentCode ORDER BY COUNT(cr.CurrencyCode) DESC
					) AS [Rank]
               FROM Currencies cr
         INNER JOIN Countries c 
		         ON cr.CurrencyCode = c.CurrencyCode
           GROUP BY c.ContinentCode, cr.CurrencyCode
             HAVING COUNT(cr.Description) > 1
	     ) AS sel
   WHERE sel.[Rank] = 1 
ORDER BY ContinentCode