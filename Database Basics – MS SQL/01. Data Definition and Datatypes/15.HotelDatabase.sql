CREATE DATABASE Hotel
GO
USE Hotel

CREATE TABLE Employees (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	Title NVARCHAR(50),
	Notes NVARCHAR(MAX)
)

CREATE TABLE Customers (
	AccountNumber INT UNIQUE NOT NULL,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	PhoneNumber NVARCHAR(50),
	EmergencyName NVARCHAR(50),
	EmergencyNumber NVARCHAR(50),
	Notes NVARCHAR(MAX)
)

CREATE TABLE RoomStatus (
	RoomStatus NVARCHAR(50) PRIMARY KEY NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE RoomTypes (
	RoomType NVARCHAR(50) PRIMARY KEY NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE BedTypes (
	BedType NVARCHAR(50) PRIMARY KEY NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE Rooms (
	RoomNumber INT PRIMARY KEY IDENTITY(1,1),
	RoomType NVARCHAR(50) NOT NULL,
	BedType NVARCHAR(50) NOT NULL,
	Rate NVARCHAR(50),
	RoomStatus NVARCHAR(50),
	Notes NVARCHAR(255)
)

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

INSERT INTO RoomStatus(RoomStatus, Notes) VALUES
('Free', NULL),
('Reserved', NULL),
('Currently not available', NULL)

INSERT INTO RoomTypes(RoomType,Notes) VALUES
('Luxury', NULL),
('Standard', NULL),
('Small', NULL)

INSERT INTO BedTypes(BedType,Notes) VALUES
('Large', NULL),
('Medium', NULL),
('Small', NULL)

INSERT INTO Rooms(RoomType, BedType, Rate, RoomStatus, Notes) VALUES
('Luxury', 'Large', 'Perfect for rich people', 'Available', NULL),
('Medium', 'Medium', NULL, 'Not available', NULL),
('Economy', 'Small', NULL, 'Available', NULL)

INSERT INTO Payments(EmployeeId,PaymentDate,AccountNumber,FirstDateOccupied,LastDateOccupied,TotalDays,AmountCharged,TaxRate,TaxAmount,PaymentTotal,Notes) VALUES
(231, NULL, 2311, NULL, NULL, 7, 5000, 0, 0, 5000, NULL),
(321, NULL, 3211, NULL,NULL, 7, 5000, 0,2000,7000,NULL),
(999, NULL, 9989, NULL,NULL, 7, 5000, 0,50,5500,NULL)

INSERT INTO Occupancies(EmployeeId,DateOccupied,AccountNumber,RoomNumber,RateApplied,PhoneCharge,Notes) VALUES
(991, NULL, 534, 8, NULL, NULL, NULL),
(561, NULL, 75, 9, NULL, NULL, NULL),
(135, NULL, 8, 10, NULL, NULL, NULL)