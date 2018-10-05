    SELECT c.CountryCode, 
           COUNT(m.MountainRange) AS MountainRanges 
      FROM Countries AS c
INNER JOIN MountainsCountries AS mc
        ON mc.CountryCode = c.CountryCode
INNER JOIN Mountains AS m
        ON m.Id = mc.MountainId
  GROUP BY c.CountryCode
    HAVING c.CountryCode IN ('BG', 'RU', 'US')