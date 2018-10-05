USE Diablo
GO
SELECT Username, RIGHT(Email, LEN(Email) - CHARINDEX('@', Email)) AS Email FROM Users
ORDER BY Email, Username