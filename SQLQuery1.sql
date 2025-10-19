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

SELECT mv.MaintenanceVisitId, mv.MaintenanceRequestId, mv.ScheduledDate, mv.Status,
       f.Brand, f.Model as EmployeeName
FROM MaintenanceVisit mv
JOIN Fridge f ON mv.FridgeId = f.FridgeId
ORDER BY mv.ScheduledDate DESC;

SELECT o.OrderId, o.CustomerID, c.FullName
FROM Orders o
LEFT JOIN Customers c ON o.CustomerID = c.CustomerID
WHERE o.Status IN ('Paid', 'Fridge Allocated');



SELECT MaintenanceVisitId FROM MaintenanceVisit;

INSERT INTO ComponentUsed (ComponentName, Quantity, Condition, MaintenanceVisitId)
VALUES
('Compressor', 1, 0, 2),
('Condenser Coil', 1, 1, 4),
('Thermostat', 1, 0, 8),
('Door Gasket', 1, 0, 7),
('Fan Motor', 2, 1, 3),
('Cooling Sensor', 1, 0, 9),
('Evaporator Coil', 1, 0, 5),
('Refrigerant Gas', 2, 0, 6),
('Electrical Wire', 3, 1, 6),
('Insulation Foam', 1, 0, 10);

SELECT CustomerId FROM Customers;

INSERT INTO CustomerNote (CustomerId, Note, CreatedAt)
VALUES
(1, 'Requested fridge delivery by next week.', '2025-10-01 09:15:00'),
(2, 'Asked for additional spare parts.', '2025-10-02 10:30:00'),
(3, 'Reported minor leakage in the fridge.', '2025-10-03 11:45:00'),
(5, 'Inquired about warranty extension.', '2025-10-04 08:50:00'),
(8, 'Requested fridge maintenance schedule.', '2025-10-05 09:40:00'),
(6, 'Asked for price quote on new fridges.', '2025-10-06 14:05:00'),
(10, 'Reported noise from the compressor.', '2025-10-07 16:30:00'),
(13, 'Requested fridge installation instructions.', '2025-10-08 09:30:00'),
(9, 'Followed up on previous maintenance request.', '2025-10-09 10:00:00'),
(12, 'Confirmed payment for delivered fridge.', '2025-10-10 11:45:00');

INSERT INTO Suppliers (Name, ContactPerson, Email, Phone, Address, IsActive)
VALUES
('CoolBrew Beverages', 'Alice Mokoena', 'contact@coolbrew.com', '0712345678', '123 Brew St, Johannesburg, Gauteng, 2000', 1),
('FreshSips Distributors', 'Sipho Khumalo', 'info@freshsips.com', '0723456789', '456 Sip Ave, Pretoria, Gauteng, 0100', 1),
('ThirstQuench Supermarket', 'Lerato Ndlovu', 'sales@thirstquench.com', '0734567890', '789 Thirst Rd, Durban, KwaZulu-Natal, 4001', 1),
('BeverageWorld Retail', 'Mandla Dlamini', 'support@beverageworld.com', '0745678901', '321 World St, Cape Town, Western Cape, 8000', 1),
('HappyDrinks Spaza', 'Nomsa Khanyile', 'owner@happydrinks.com', '0756789012', '654 Happy Ln, Soweto, Gauteng, 1800', 1),
('Urban Juice Bar', 'Thabo Mthembu', 'contact@urbanjuice.com', '0767890123', '987 Urban Rd, Polokwane, Limpopo, 0700', 1),
('Fresh Fizz Distributors', 'Zanele Mokoena', 'hello@freshfizz.com', '0778901234', '159 Fizz Blvd, Bloemfontein, Free State, 9301', 1),
('Liquid Refreshers Ltd', 'Kgosi Motloung', 'info@liquidrefreshers.com', '0789012345', '753 Liquid St, Nelspruit, Mpumalanga, 1200', 1),
('ChillPoint Beverages', 'Sibusiso Dube', 'sales@chillpoint.com', '0790123456', '852 Chill Ave, East London, Eastern Cape, 5200', 1),
('Frosty Drinks Co.', 'Lindiwe Sithole', 'contact@frostydrinks.com', '0701234567', '951 Frosty Rd, Kimberley, Northern Cape, 8300', 1);



INSERT INTO PurchaseOrders (PONumber, OrderDate, QuotationID, SupplierID, TotalAmount, Status, ExpectedDeliveryDate, DeliveryAddress, SpecialInstructions, IsActive)
VALUES
('PO-2025-001', '2025-09-11', 13, 1, 50000.00, 'Ordered', '2025-10-01', '12 Market Street, Johannesburg', 'Handle with care', 1),
('PO-2025-002', '2025-09-12', 12, 11, 75000.00, 'Ordered', '2025-10-03', '45 Industrial Rd, Pretoria', 'Fragile items', 1),
('PO-2025-003', '2025-09-13', 11, 3, 62000.00, 'Ordered', '2025-10-05', '78 Main Ave, Cape Town', 'Deliver in the morning', 1),
('PO-2025-004', '2025-09-14', 4, 4, 88000.00, 'Ordered', '2025-10-07', '33 High St, Durban', 'Keep upright', 1),
('PO-2025-005', '2025-09-15', 5, 5, 45000.00, 'Ordered', '2025-10-09', '55 Commerce Rd, Bloemfontein', 'Check seals', 1),
('PO-2025-006', '2025-09-16', 6, 6, 54000.00, 'Ordered', '2025-10-11', '99 Sunshine Blvd, Nelspruit', 'Verify quantity', 1),
('PO-2025-007', '2025-09-17', 7, 7, 68000.00, 'Ordered', '2025-10-13', '101 Garden St, Port Elizabeth', 'Call before delivery', 1),
('PO-2025-008', '2025-09-18', 8, 8, 72000.00, 'Ordered', '2025-10-15', '202 River Rd, Kimberley', 'Include manuals', 1),
('PO-2025-009', '2025-09-19', 9, 9, 81000.00, 'Ordered', '2025-10-17', '11 Ocean View, East London', 'Inspect units', 1),
('PO-2025-010', '2025-09-20', 10, 10, 95000.00, 'Ordered', '2025-10-19', '66 Hill Rd, Polokwane', 'Deliver by 10 AM', 1);

SELECT PurchaseOrderId FROM PurchaseOrders;
SELECT QuotationId FROM Quotations;

INSERT INTO DeliveryNotes (DeliveryNumber, DeliveryDate, QuantityDelivered, ReceivedBy, IsVerified, VerificationDate, IsReceivedInInventory, InventoryReceiptDate, PurchaseOrderId, SupplierId, Notes, IsActive)
VALUES
('DN-1001', '2025-10-01', 5, 'John Doe', 1, '2025-10-02 10:00:00', 1, '2025-10-02 11:00:00', 5, 1, 'Delivered on time, all fridges in good condition.', 1),
('DN-1002', '2025-10-03', 3, 'Jane Smith', 1, '2025-10-04 09:30:00', 1, '2025-10-04 10:00:00', 7, 3, 'Minor scratches on one fridge, noted.', 1),
('DN-1003', '2025-10-05', 7, 'Mike Brown', 0, NULL, 0, NULL, 8, 4, 'Pending verification, some boxes unopened.', 1),
('DN-1004', '2025-10-07', 2, 'Alice Green', 1, '2025-10-08 14:00:00', 1, '2025-10-08 14:30:00', 9, 5, 'Fridges delivered and installed.', 1),
('DN-1005', '2025-10-09', 6, 'Bob White', 0, NULL, 0, NULL, 10, 6, 'Awaiting inventory receipt.', 1),
('DN-1006', '2025-10-11', 4, 'Chris Black', 1, '2025-10-12 11:15:00', 1, '2025-10-12 12:00:00', 11, 7, 'All units verified, no issues.', 1),
('DN-1007', '2025-10-13', 5, 'Diana Blue', 0, NULL, 0, NULL, 12, 8, 'Delivered, pending customer confirmation.', 1),
('DN-1008', '2025-10-15', 3, 'Evan Gray', 1, '2025-10-16 09:45:00', 1, '2025-10-16 10:15:00', 13, 9, 'Received in inventory, condition good.', 1),
('DN-1009', '2025-10-17', 8, 'Fiona Red', 1, '2025-10-18 15:30:00', 1, '2025-10-18 16:00:00', 14, 10, 'Delivered all units successfully.', 1),
('DN-1010', '2025-10-19', 2, 'George Yellow', 0, NULL, 0, NULL, 6, 11, 'Some items still in transit.', 1);

INSERT INTO Quotations (RequestForQuotationId, ReceivedDate, QuotationAmount, Status, SupplierId)
VALUES
(1, '2025-09-01', 50000.00, 'Accepted', 1),
(2, '2025-09-02', 75000.00, 'Accepted', 3),
(3, '2025-09-03', 62000.00, 'Accepted', 4),
(4, '2025-09-04', 88000.00, 'Accepted', 11),
(5, '2025-09-05', 45000.00, 'Accepted', 5),
(6, '2025-09-06', 54000.00, 'Accepted', 6),
(7, '2025-09-07', 68000.00, 'Accepted', 7),
(8, '2025-09-08', 72000.00, 'Accepted', 8),
(9, '2025-09-09', 81000.00, 'Accepted', 9),
(10, '2025-09-10', 95000.00, 'Accepted', 10);

SELECT SupplierId FROM Suppliers;

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