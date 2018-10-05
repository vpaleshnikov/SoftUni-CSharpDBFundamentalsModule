CREATE TABLE Users (
	Id BIGINT IDENTITY NOT NULL,
	Username VARCHAR(30) UNIQUE NOT NULL,
	Password VARCHAR(26) NOT NULL,
	ProfilePicture VARBINARY(MAX),
	LastLoginTime DATETIME,
	IsDeleted BIT,
	CONSTRAINT PK_Users PRIMARY KEY (Id)
)

ALTER TABLE Users
ADD CONSTRAINT CK_ProfilePictureSize CHECK (DATALENGTH(ProfilePicture) <= 900 * 1024)

ALTER TABLE Users
DROP CONSTRAINT PK_Users

ALTER TABLE Users
ADD CONSTRAINT PK_Users PRIMARY KEY (Id, Username)

ALTER TABLE Users
ADD CONSTRAINT CK_PasswordSize CHECK (DATALENGTH(Password) >= 5)

ALTER TABLE Users
ADD DEFAULT GETDATE() FOR LastLoginTime

INSERT INTO Users (Username, Password, IsDeleted)
VALUES 
('Dimitar', '12345', 0),
('Ivan', '12346', 1),
('Petyr', '12347', 0),
('Ivaylo', '12348', 1),
('Georgi', '12349', 0)