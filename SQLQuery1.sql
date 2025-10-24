USE [GRP-03-16];
GO

INSERT INTO Locations (Address, City, Province, PostalCode, IsActive)
VALUES
('12 Main Street', 'Pretoria', 'Gauteng', '0001', 1),
('45 Riverside Drive', 'Johannesburg', 'Gauteng', '2000', 1),
('78 Ocean View Ave', 'Durban', 'KwaZulu-Natal', '4001', 1),
('33 Hilltop Road', 'Cape Town', 'Western Cape', '8001', 1),
('56 Greenway Blvd', 'Port Elizabeth', 'Eastern Cape', '6001', 1),
('101 Sunset Lane', 'Bloemfontein', 'Free State', '9301', 1),
('22 Oakwood Drive', 'Polokwane', 'Limpopo', '0700', 1),
('89 Cedar Street', 'Nelspruit', 'Mpumalanga', '1200', 1),
('7 Riverbend Ave', 'Kimberley', 'Northern Cape', '8301', 1),
('15 Maple Road', 'Mthatha', 'Eastern Cape', '5100', 1);

SELECT * FROM Locations;

INSERT INTO Customers(FullName, Email, LocationId, PhoneNumber, RegistrationDate, ShopType, SecurityQuestion, SecurityAnswerHash, IsVerified, IsActive, CreatedAt)
VALUES
('CoolBrew Beverages', 'contact@coolbrew.com', 1, '0712345678', '2025-01-10', 1, 'Favorite color?', 'hash1', 1, 1, '2025-01-10 09:15:00'),
('FreshSips Distributors', 'info@freshsips.com', 2, '0723456789', '2025-02-15', 1, 'Favorite color?', 'hash2', 1, 1, '2025-02-15 10:20:00'),
('SipNChill Cafe', 'hello@sipnchill.com', 11, '0734567890', '2025-03-20', 3, 'Pet''s name?', 'hash3', 1, 1, '2025-03-20 11:35:00'),
('ThirstQuench Supermarket', 'sales@thirstquench.com', 1, '0745678901', '2025-04-05', 4, 'Birth city?', 'hash4', 1, 1, '2025-04-05 08:50:00'),
('BeverageWorld Retail', 'support@beverageworld.com', 6, '0756789012', '2025-05-12', 2, 'Mother''s maiden name?', 'hash5', 1, 1, '2025-05-12 09:40:00'),
('HappyDrinks Spaza', 'owner@happydrinks.com', 3, '0767890123', '2025-06-18', 5, 'Favorite teacher?', 'hash6', 1, 1, '2025-06-18 14:05:00'),
('Urban Juice Bar', 'contact@urbanjuice.com', 10, '0778901234', '2025-07-22', 3, 'Best friend''s name?', 'hash7', 1, 1, '2025-07-22 16:30:00'),
('John Doe', 'johndoe@email.com', 7, '0789012345', '2025-08-30', 0, 'Favorite color?', 'hash8', 0, 1, '2025-08-30 12:15:00'),
('Jane Smith', 'janesmith@email.com', 3, '0790123456', '2025-09-10', 0, 'Pet''s name?', 'hash9', 0, 1, '2025-09-10 10:45:00'),
('Fresh Fizz Distributors', 'hello@freshfizz.com', 8, '0701234567', '2025-10-05', 1, 'Birth city?', 'hash10', 1, 1, '2025-10-05 11:50:00');

SELECT CustomerId  FROM Customers;

INSERT INTO Carts(CustomerID, IsActive)
VALUES
(14, 1),
(7, 1),
(11, 1),
(5, 1),
(8, 1),
(6, 1),
(10, 1),
(13, 1),
(9, 1),
(12, 1);

SELECT FridgeId FROM Fridge;
SELECT CartId FROM Carts;

INSERT INTO CartItems (CartId, FridgeId, Quantity, Price, IsDeleted)
VALUES
(1, 1, 2, 12000.00, 0),
(2, 2, 1, 8500.00, 0),
(3, 3, 3, 15000.00, 0),
(13, 4, 1, 9500.00, 0),
(15, 5, 2, 11000.00, 0),
(11, 6, 1, 7800.00, 0),
(14, 7, 4, 20000.00, 0),
(18, 8, 1, 6000.00, 0),
(16, 9, 2, 12500.00, 0),
(12, 10, 1, 9000.00, 0);

SELECT EmployeeId, FullName FROM Employees;

SELECT MaintenanceRequestId FROM MaintenanceRequest;

SELECT 
    mv.MaintenanceVisitId,
    mv.MaintenanceRequestId,
    mv.ScheduledDate,
    mv.Status,
    f.Brand,
    f.Model,
    c.FullName AS CustomerName
FROM MaintenanceVisit mv
JOIN Fridge f ON mv.FridgeId = f.FridgeId
JOIN FridgeAllocation fa ON fa.FridgeId = f.FridgeId
JOIN Customers c ON fa.CustomerID = c.CustomerID
ORDER BY mv.ScheduledDate DESC;



SELECT o.OrderId, o.CustomerID, c.FullName
FROM Orders o
LEFT JOIN Customers c ON o.CustomerID = c.CustomerID
WHERE o.Status IN ('Paid', 'Fridge Allocated');



SELECT MaintenanceVisitId FROM MaintenanceVisit;

SELECT CustomerId FROM Customers;

SELECT FridgeId, FridgeType FROM Fridge;



SELECT SupplierId FROM Suppliers;
SELECT * FROM Inventory;

DELETE FROM Customers;
DELETE FROM Fridge;
DELETE FROM CartItems;
DELETE FROM Carts;
DELETE FROM Suppliers;
DELETE FROM Inventory;
DELETE FROM PurchaseRequests;
DELETE FROM RequestsForQuotation;
DELETE FROM Quotations;
DELETE FROM PurchaseOrders;
DELETE FROM DeliveryNotes;
DELETE FROM Faults;

SELECT FridgeId FROM Fridge;
SELECT CustomerId FROM Customers;