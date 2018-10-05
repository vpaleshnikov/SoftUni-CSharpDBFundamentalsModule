CREATE DATABASE Persons
GO
USE Persons
GO
CREATE TABLE Passports (
	PassportID INT IDENTITY(101, 1),
	PassportNumber VARCHAR(50) NOT NULL,
	CONSTRAINT PK_Passports PRIMARY KEY (PassportID)	
)

CREATE TABLE Persons (
	PersonID INT IDENTITY,
	FirstName VARCHAR(50) NOT NULL,
	Salary DECIMAL(10,2),
	PassportID INT NOT NULL,

	CONSTRAINT PK_Persons PRIMARY KEY (PersonID),

	CONSTRAINT FK_Persons_Passports 
	FOREIGN KEY (PassportID) 
	REFERENCES Passports(PassportID)
)

INSERT INTO Passports (PassportNumber) VALUES
('N34FG21B'),
('K65LO4R7'),
('ZE657QP2')

INSERT INTO Persons (FirstName, Salary, PassportID) VALUES
('Roberto', 43300.00, 102),
('Tom', 56100.00, 103),
('Yana', 60200.00, 101)