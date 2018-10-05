SELECT COUNT(CountryCode) AS CountryCode
  FROM Countries
 WHERE CountryCode NOT IN 
       (
	   SELECT CountryCode 
	   FROM MountainsCountries
	   )