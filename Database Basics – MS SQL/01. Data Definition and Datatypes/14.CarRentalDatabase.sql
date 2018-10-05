CREATE DATABASE CarRental
GO
USE CarRental

CREATE TABLE Categories (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	CategoryName NVARCHAR(50) NOT NULL,
	DailyRate DECIMAL(10,2),
	WeeklyRate DECIMAL(10,2),
	MonthlyRate DECIMAL(10,2),
	WeekendRate DECIMAL(10,2)
)

CREATE TABLE Cars (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	PlateNumber NVARCHAR(50) NOT NULL,
	Manufacturer NVARCHAR(50) NOT NULL,
	Model NVARCHAR(50) NOT NULL,
	CarYear DATE,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
	Doors INT,
	Picture VARBINARY(MAX),
	Condition NVARCHAR(255),
	Available BIT NOT NULL
)

CREATE TABLE Employees (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	Title NVARCHAR(255),
	Notes NVARCHAR(MAX)
)

CREATE TABLE Customers (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	DriverLicenceNumber INT UNIQUE NOT NULL,
	FullName NVARCHAR(100) NOT NULL,
	Address NVARCHAR(MAX),
	ZIPCode INT,
	Notes NVARCHAR(MAX)
)

CREATE TABLE RentalOrders (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id) NOT NULL,
	CustomerId INT FOREIGN KEY REFERENCES Customers(Id) NOT NULL,
	CarId INT FOREIGN KEY REFERENCES Cars(Id) NOT NULL,
	TankLevel DECIMAL(10,2),
	KilometrageStart DECIMAL(10,1),
	KilometrageEnd DECIMAL(10,1),
	TotalKilometrage DECIMAL(10,1),
	StartDate DATETIME,
	EndDate DATETIME,
	TotalDays INT,
	RateApplied NVARCHAR(50),
	TaxRate DECIMAL(10,2),
	OrderStatus NVARCHAR(50),
	Notes NVARCHAR(MAX)
)

INSERT INTO Categories (CategoryName) VALUES
('Sport'),
('Comfort'),
('Limousine')

INSERT INTO Cars (PlateNumber, Manufacturer, Model, CategoryId, Available) VALUES
('B4843HP', 'Opel', 'Astra', 2, 0),
('P5833CP', 'VW', 'Golf', 1, 1),
('CA8823HM', 'Jeep', 'Wrangler', 3, 0)

INSERT INTO Employees (FirstName, LastName) VALUES
('Ivan', 'Draganov'),
('Petar', 'Nikolov'),
('Stefan', 'Valeriev')

INSERT INTO Customers (DriverLicenceNumber, FullName) VALUES
('123456789', 'Todor Ivanov'),
('234567891', 'Mladen Naydenov'),
('345678912', 'Kalin Terziev')

INSERT INTO RentalOrders (EmployeeId, CustomerId, CarId) VALUES
(1, 2, 3),
(2, 3, 1),
(3, 1, 2)