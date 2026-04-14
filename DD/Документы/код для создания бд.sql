CREATE DATABASE PharmacyDB;
GO
USE PharmacyDB;

-- Роли
CREATE TABLE Roles (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(50)
);

INSERT INTO Roles VALUES 
('Администратор'),
('Менеджер'),
('Пользователь');

-- Пользователи
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY,
    Login NVARCHAR(50),
    Password NVARCHAR(50),
    FullName NVARCHAR(100),
    RoleId INT FOREIGN KEY REFERENCES Roles(Id)
);

INSERT INTO Users VALUES
('admin','123','Администратор',1),
('manager','123','Менеджер',2),
('user','123','Пользователь',3);

-- Категории
CREATE TABLE Categories (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100)
);

INSERT INTO Categories VALUES
('Обезболивающие'),
('Антибиотики'),
('Витамины');

-- Товары
CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100),
    CategoryId INT FOREIGN KEY REFERENCES Categories(Id),
    Description NVARCHAR(255),
    Price DECIMAL(10,2),
    Quantity INT,
    Discount INT DEFAULT 0,
    ImagePath NVARCHAR(255)
);

-- Заказы
CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY,
    UserId INT FOREIGN KEY REFERENCES Users(Id),
    OrderDate DATETIME,
    DeliveryDate DATETIME,
    Address NVARCHAR(255),
    Status NVARCHAR(50)
);

-- Состав заказа
CREATE TABLE OrderItems (
    Id INT PRIMARY KEY IDENTITY,
    OrderId INT FOREIGN KEY REFERENCES Orders(Id),
    ProductId INT FOREIGN KEY REFERENCES Products(Id),
    Quantity INT
);