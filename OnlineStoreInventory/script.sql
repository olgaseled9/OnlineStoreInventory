
/**
  Таблица для категорий товаров
 Id: Уникальный идентификатор категории.
 Name: Название категории товара.
 Description: Описание категории товара.
*/
CREATE TABLE Category
(
    Id          INT PRIMARY KEY IDENTITY (1,1), 
    Name        NVARCHAR(100) NOT NULL,         
    Description NVARCHAR(255)                   
);
GO

/**
  Таблица товаров
Id: Уникальный идентификатор товара.
Name: Название товара.
CategoryId: Идентификатор категории товара (внешний ключ).
Barcode: Штрихкод товара.
Price: Цена товара.
Weight: Вес товара.
Dimensions: Размеры товара.
MinStock: Минимальный остаток товара.
 */
CREATE TABLE Product (
                         Id INT PRIMARY KEY IDENTITY(1,1),
                         Name NVARCHAR(100) NOT NULL,
                         CategoryId INT,
                         Barcode NVARCHAR(50),
                         Price DECIMAL(10, 2),
                         Weight FLOAT,
                         Dimensions NVARCHAR(50),
                         MinStock INT,
                         FOREIGN KEY (CategoryId) REFERENCES Category(Id)
);
GO



/**
  Таблица для складских запасов
Id: Уникальный идентификатор записи.
ProductId: Идентификатор товара (внешний ключ).
Quantity: Количество товара на складе.
Location: Местоположение товара на складе.
IsInReserve: Флаг, указывающий, находится ли товар в резервной зоне.
 */
CREATE TABLE Stock (
                       Id INT PRIMARY KEY IDENTITY(1,1),
                       ProductId INT,
                       Quantity INT,
                       Location NVARCHAR(100),
                       IsInReserve BIT,
                       FOREIGN KEY (ProductId) REFERENCES Product(Id)
);

GO

-- Таблица для поставок товаров
/**
  Id: Уникальный идентификатор поставки.
Date: Дата поставки.
Supplier: Имя поставщика.
ProductId: Идентификатор товара (внешний ключ).
Quantity: Количество поставленных товаров.
TotalCost: Общая стоимость поставки
 */
CREATE TABLE Supply (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        Date DATETIME,
                        Supplier NVARCHAR(100),
                        ProductId INT,
                        Quantity INT,
                        TotalCost DECIMAL(10, 2),
                        FOREIGN KEY (ProductId) REFERENCES Product(Id)
);

GO

/**
Id: Уникальный идентификатор отгрузки.
Date: Дата отгрузки.
ProductId: Идентификатор товара (внешний ключ).
Quantity: Количество отгружаемого товара.
DeliveryAddress: Адрес доставки.
ShipmentCost: Стоимость доставки.
 */
CREATE TABLE Shipment (
                          Id INT PRIMARY KEY IDENTITY(1,1),
                          Date DATETIME,
                          ProductId INT,
                          Quantity INT,
                          DeliveryAddress NVARCHAR(255),
                          ShipmentCost DECIMAL(10, 2),
                          FOREIGN KEY (ProductId) REFERENCES Product(Id)
);
GO



/**
  Таблица для заказов на закупку товаров
Id: Уникальный идентификатор заказа.
Date: Дата создания заказа.
ProductId: Идентификатор товара (внешний ключ).
Quantity: Количество заказываемых товаров.
Status: Статус заказа (например, «В ожидании», «Выполнен»).

 */
CREATE TABLE PurchaseOrder (
                               Id INT PRIMARY KEY IDENTITY(1,1),
                               Date DATETIME,
                               ProductId INT,
                               Quantity INT,
                               Status NVARCHAR(50),
                               FOREIGN KEY (ProductId) REFERENCES Product(Id)
);
GO
/**
  Таблица для отчетов
  Id: Уникальный идентификатор отчета.
Date: Дата создания отчета.
Type: Тип отчета (например, «наличие товаров», «поставки»).
Data: Данные отчета в формате JSON или XML.
 */

CREATE TABLE Report (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        Date DATETIME,
                        Type NVARCHAR(50),
                        Data NVARCHAR(MAX)
);


INSERT INTO Category (Name, Description)
VALUES ('Electronics', 'Электронные товары'),
       ('Furniture', 'Мебель'),
       ('Clothing', 'Одежда');
GO

INSERT INTO Product (Name, CategoryId, Barcode, Price, Weight, Dimensions, MinStock)
VALUES ('Laptop', 1, '1234567890', 1200.00, 2.5, '35x25x3', 10),
       ('Sofa', 2, '9876543210', 500.00, 50.0, '200x100x90', 5),
       ('T-Shirt', 3, '1122334455', 20.00, 0.2, 'M', 50);
GO
INSERT INTO Stock (ProductId, Quantity, Location, IsInReserve)
VALUES (1, 15, 'A1', 0),
       (2, 3, 'B2', 0),
       (3, 100, 'C3', 1);
GO


INSERT INTO Supply (Date, Supplier, ProductId, Quantity, TotalCost)
VALUES ('2025-01-01', 'Supplier1', 1, 10, 12000.00),
       ('2025-01-02', 'Supplier2', 2, 5, 2500.00),
       ('2025-01-03', 'Supplier3', 3, 200, 4000.00);
GO
INSERT INTO Shipment (Date, ProductId, Quantity, DeliveryAddress, ShipmentCost)
VALUES ('2025-01-10', 1, 5, 'Address1', 30.00),
       ('2025-01-11', 2, 2, 'Address2', 50.00),
       ('2025-01-12', 3, 50, 'Address3', 100.00);
GO
INSERT INTO PurchaseOrder (Date, ProductId, Quantity, Status)
VALUES ('2025-01-18T00:00:00', 1, 10, 'In Progress'),
       ('2025-01-21T00:00:00', 2, 10, 'Completed'),
       ('2025-01-22T00:00:00', 3, 150, 'In Progress');
GO
INSERT INTO Report (Date, Type, Data)
VALUES ('2025-01-20T00:00:00', 'Inventory Report', 'Data in JSON or XML format'),
       ('2025-01-21T00:00:00', 'Supply Report', 'Data in JSON or XML format');
GO

GO

