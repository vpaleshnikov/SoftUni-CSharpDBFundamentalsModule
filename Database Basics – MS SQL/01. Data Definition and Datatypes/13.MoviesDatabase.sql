CREATE DATABASE Movies

CREATE TABLE Directors (
	Id INT IDENTITY NOT NULL,
	DirectorName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX),
	CONSTRAINT PK_Directors PRIMARY KEY (Id)
)

CREATE TABLE Genres (
	Id INT IDENTITY NOT NULL,
	GenreName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
	CONSTRAINT PK_Genres PRIMARY KEY (Id)
)

CREATE TABLE Categories (
	Id INT IDENTITY NOT NULL,
	CategoryName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
	CONSTRAINT PK_Categories PRIMARY KEY (Id)
)

CREATE TABLE Movies (
	Id INT IDENTITY NOT NULL,
	Title NVARCHAR(50) NOT NULL,
	DirectorId INT FOREIGN KEY REFERENCES Directors(Id) NOT NULL,
	CopyrightYear INT,
	Length NVARCHAR(30),
	GenreID INT FOREIGN KEY REFERENCES Genres(Id) NOT NULL,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
	Rating DECIMAL(10,2),
	Notes NVARCHAR(MAX),
	CONSTRAINT PK_Movies PRIMARY KEY (Id)
)

INSERT INTO Directors VALUES
('Dimitar Dimitrov', NULL),
('Ivaylo Dimitrov', NULL),
('Georgi Dimitrov', NULL),
('Sava Dimitrov', NULL),
('Naum Dimitrov', NULL)

INSERT INTO Genres VALUES
('Action', NULL),
('SI-FI', NULL),
('Comedy', NULL),
('Horor', NULL),
('Adventure', NULL)

INSERT INTO Categories VALUES
('Comedy film', NULL),
('Action film', NULL),
('Erotic film', NULL),
('Dance film', NULL),
('Documentary film', NULL)

INSERT INTO Movies VALUES
('Samurai', 3, NULL, NULL, 1, 5, NULL, NULL),
('The Matrix', 2, NULL, NULL, 2, 4, NULL, NULL),
('Colombiana', 5, NULL, NULL, 3, 3, NULL, NULL),
('Die Hard', 4, NULL, NULL, 4, 2, NULL, NULL),
('Broken Arrow', 1, NULL, NULL, 5, 1, NULL, NULL)