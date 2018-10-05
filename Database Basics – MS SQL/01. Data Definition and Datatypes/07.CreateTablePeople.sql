CREATE TABLE People (
	Id INT UNIQUE IDENTITY(1,1),
	Name VARCHAR(200) NOT NULL,
	Picture VARBINARY CHECK(DATALENGTH(Picture) < 900 * 1024),
	Height DECIMAL(10, 2),
	Weight DECIMAL(10, 2),
	Gender VARCHAR(1) CHECK (Gender = 'm' OR Gender = 'f') NOT NULL,
	Birthdate DATE NOT NULL,
	Biography VARCHAR(MAX)
)

INSERT INTO People (Name, Picture, Height, Weight, Gender, Birthdate, Biography)
VALUES ('Milena', NULL, 1.55, 46, 'f', '1985-12-02', NULL)

INSERT INTO People (Name, Picture, Height, Weight, Gender, Birthdate, Biography)
VALUES ('Ivan', NULL, 1.96, 78, 'm', '1969-05-22', NULL)

INSERT INTO People (Name, Picture, Height, Weight, Gender, Birthdate, Biography)
VALUES ('Alex', NULL, 1.65, 42, 'f', '1982-02-01', NULL)

INSERT INTO People (Name, Picture, Height, Weight, Gender, Birthdate, Biography)
VALUES ('Dimitar', NULL, 1.85, 90, 'm', '1995-07-07', NULL)

INSERT INTO People (Name, Picture, Height, Weight, Gender, Birthdate, Biography)
VALUES ('Gergana', NULL, 1.71, 60, 'f', '1991-10-12', NULL)