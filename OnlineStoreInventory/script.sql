
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


-- Заполнение таблицы Category
INSERT INTO Category (Name, Description)
VALUES
    ('Электроника', 'Электронные устройства и гаджеты'),
    ('Мебель', 'Домашняя и офисная мебель'),
    ('Одежда', 'Модная и стильная одежда');
GO

-- Заполнение таблицы Product
-- Обратите внимание: CategoryId соответствует порядку записей в таблице Category (1 – Электроника, 2 – Мебель, 3 – Одежда)
INSERT INTO Product (Name, CategoryId, Barcode, Price, Weight, Dimensions, MinStock)
VALUES
    ('Смартфон', 1, '1111111111', 15000.00, 0.2, '150x75x8 мм', 10),
    ('Ноутбук', 1, '2222222222', 45000.00, 1.5, '350x250x20 мм', 5),
    ('Диван', 2, '3333333333', 30000.00, 50.0, '200x90x100 см', 2),
    ('Стул', 2, '4444444444', 5000.00, 7.0, '40x40x90 см', 15),
    ('Футболка', 3, '5555555555', 800.00, 0.3, 'Размер M', 30),
    ('Джинсы', 3, '6666666666', 2000.00, 0.7, 'Размер 42', 20);
GO

-- Заполнение таблицы PurchaseOrders (Заказы на закупку)
INSERT INTO PurchaseOrders (Date, ProductId, Quantity, Status)
VALUES
    ('2023-01-15', 1, 20, 'В ожидании'),
    ('2023-02-10', 3, 5, 'Завершен'),
    ('2023-03-05', 5, 50, 'В процессе');
GO

-- Заполнение таблицы Report (Отчёты)
INSERT INTO Report (Date, Type, Data)
VALUES
    ('2023-04-01', 'Инвентарный отчёт', '{ "Всего товаров": 100, "Товаров с низким запасом": 5 }'),
    ('2023-04-15', 'Отчёт поставок', '{ "Всего поставок": 20, "Общая стоимость": 500000 }');
GO

-- Заполнение таблицы Shipment (Отгрузки)
INSERT INTO Shipment (Date, ProductId, Quantity, DeliveryAddress, ShipmentCost)
VALUES
    ('2023-04-20', 2, 3, 'ул. Ленина, д. 10, Москва', 1500.00),
    ('2023-04-22', 4, 10, 'пр. Мира, д. 5, Санкт-Петербург', 3000.00);
GO

-- Заполнение таблицы Stock (Складские запасы)
INSERT INTO Stock (ProductId, Quantity, Location, IsInReserve)
VALUES
    (1, 8, 'Склад №1', 0),
    (2, 4, 'Склад №1', 0),
    (3, 1, 'Склад №2', 1),
    (4, 20, 'Склад №2', 0),
    (5, 25, 'Склад №3', 0),
    (6, 15, 'Склад №3', 0);
GO

-- Заполнение таблицы Supply (Поставки)
INSERT INTO Supply (Date, Supplier, ProductId, Quantity, TotalCost)
VALUES
    ('2023-04-10', 'Поставщик А', 1, 20, 300000.00),
    ('2023-04-12', 'Поставщик Б', 3, 5, 150000.00),
    ('2023-04-18', 'Поставщик В', 5, 50, 40000.00);
GO


