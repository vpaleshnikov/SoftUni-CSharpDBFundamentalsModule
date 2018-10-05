CREATE TABLE Payments
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	EmployeeId INT UNIQUE NOT NULL,
	PaymentDate DATE,
	AccountNumber INT NOT NULL,
	FirstDateOccupied DATE,
	LastDateOccupied DATE,
	TotalDays INT NOT NULL,
	AmountCharged INT NOT NULL,
	TaxRate INT,
	TaxAmount INT,
	PaymentTotal INT NOT NULL,
	Notes NVARCHAR(MAX)
)

INSERT INTO Payments(EmployeeId,PaymentDate,AccountNumber,FirstDateOccupied,LastDateOccupied,TotalDays,AmountCharged,TaxRate,TaxAmount,PaymentTotal,Notes) VALUES
(231, NULL, 2311, NULL, NULL, 7, 5000, 0, 0, 5000, NULL),
(321, NULL, 3211, NULL,NULL, 7, 5000, 0,2000,7000,NULL),
(999, NULL, 9989, NULL,NULL, 7, 5000, 0,50,5500,NULL)

UPDATE Payments
SET TaxRate *= 1.03
  
SELECT TaxRate FROM Payments