CREATE DATABASE OnlineStore
GO
USE OnlineStore
GO
CREATE TABLE ItemTypes (
	ItemTypeID INT PRIMARY KEY IDENTITY,
	Name VARCHAR(50) NOT NULL
)

CREATE TABLE Items (
	ItemID INT PRIMARY KEY IDENTITY,
	Name VARCHAR(50) NOT NULL,
	ItemTypeID INT NOT NULL FOREIGN KEY REFERENCES ItemTypes(ItemTypeID)
)

CREATE TABLE Cities (
	CityID INT PRIMARY KEY IDENTITY,
	Name VARCHAR(50) NOT NULL
)

CREATE TABLE Customers (
	CustomerID INT PRIMARY KEY IDENTITY,
	Name VARCHAR(50) NOT NULL,
	Birthday DATE,
	CityID INT NOT NULL FOREIGN KEY REFERENCES Cities(CityID)
)

CREATE TABLE Orders (
	OrderID INT PRIMARY KEY IDENTITY,
	CustomerID INT NOT NULL FOREIGN KEY REFERENCES Customers(CustomerID)
)

CREATE TABLE OrderItems (
	OrderID INT,
	ItemID INT,

	CONSTRAINT PK_OrdersItems PRIMARY KEY (OrderID, ItemID),

	CONSTRAINT FK_OrderItems_Orders 
	FOREIGN KEY (OrderID) 
	REFERENCES Orders(OrderID),

	CONSTRAINT FK_OrderItems_Items
	FOREIGN KEY (ItemID)
	REFERENCES Items(ItemID)
)