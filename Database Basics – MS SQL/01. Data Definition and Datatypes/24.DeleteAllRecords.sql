CREATE TABLE Occupancies
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	EmployeeId INT UNIQUE NOT NULL,
	DateOccupied DATE,
	AccountNumber INT NOT NULL,
	RoomNumber INT NOT NULL,
	RateApplied INT,
	PhoneCharge INT,
	Notes NVARCHAR(255)
)

INSERT INTO Occupancies(EmployeeId,DateOccupied,AccountNumber,RoomNumber,RateApplied,PhoneCharge,Notes) VALUES
(991, NULL, 534, 8, NULL, NULL, NULL),
(561, NULL, 75, 9, NULL, NULL, NULL),
(135, NULL, 8, 10, NULL, NULL, NULL)

TRUNCATE TABLE Occupancies