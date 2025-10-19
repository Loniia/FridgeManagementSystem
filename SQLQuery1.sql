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

INSERT INTO MaintenanceRequest (FridgeId, RequestDate, TaskStatus, IsActive)
VALUES
(1, '2025-10-01', 0, 1), -- Pending
(2, '2025-10-03', 1, 1), -- InProgress
(3, '2025-10-05', 2, 1), -- Complete
(4, '2025-10-07', 0, 1), -- Pending
(5, '2025-10-09', 3, 1), -- Cancelled
(6, '2025-10-11', 0, 1), -- Pending
(7, '2025-10-13', 4, 1), -- Scheduled
(8, '2025-10-15', 5, 1), -- OnHold
(9, '2025-10-17', 6, 1), -- Rescheduled
(10, '2025-10-19', 1, 1); -- InProgress

SELECT MaintenanceRequestId FROM MaintenanceRequest;

SELECT mv.MaintenanceVisitId, mv.MaintenanceRequestId, mv.ScheduledDate, mv.Status,
       f.Brand, f.Model as EmployeeName
FROM MaintenanceVisit mv
JOIN Fridge f ON mv.FridgeId = f.FridgeId
ORDER BY mv.ScheduledDate DESC;


INSERT INTO MaintenanceVisit (FridgeId, EmployeeID, MaintenanceRequestId, ScheduledDate, ScheduledTime, VisitNotes, Status)
VALUES
(1, 4, 22, '2025-10-20', '09:00:00', 'Routine check and cleaning.', 0),
(2, 4, 21, '2025-10-21', '10:30:00', 'Replaced faulty compressor.', 2),
(3, 4, 13, '2025-10-22', '11:15:00', 'Checked cooling efficiency.', 0),
(4, 4, 14, '2025-10-23', '08:45:00', 'Cleaned condenser coils.', 1),
(5, 4, 16, '2025-10-24', '14:00:00', 'Lubricated moving parts.', 0),
(6, 4, 20, '2025-10-25', '13:20:00', 'Inspected electrical connections.', 0),
(7, 4, 17, '2025-10-26', '15:10:00', 'Replaced door gasket.', 2),
(8, 4, 18, '2025-10-27', '09:30:00', 'Checked thermostat settings.', 0),
(9, 4, 19, '2025-10-28', '10:00:00', 'Fixed minor leakage.', 1),
(10, 4, 15, '2025-10-29', '11:45:00', 'Full maintenance inspection.', 0);

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

INSERT INTO Inventory (FridgeID, Quantity, LastUpdated)
VALUES
(1, 10, '2025-01-02'),
(2, 8, '2025-01-05'),
(3, 12, '2025-01-10'),
(4, 7, '2025-01-15'),
(5, 9, '2025-02-01'),
(6, 6, '2025-02-10'),
(7, 15, '2025-02-20'),
(8, 11, '2025-03-01'),
(9, 13, '2025-03-10'),
(10, 5, '2025-03-20');

INSERT INTO PurchaseRequests 
(FridgeId, InventoryID, RequestDate, ItemFullNames, RequestBy, AssignedToRole, Status, RequestType, RequestNumber, Quantity, IsActive, IsViewed, ViewedDate)
VALUES
(NULL, 1, '2025-01-03', 'Commercial Beverage Fridge', 'Lerato Mokoena', 'PurchasingManager', 'Approved', 'Equipment', 'PR-2025-001', 5, 1, 0, NULL),
(NULL, 2, '2025-01-10', 'Mini Cooler', 'John Nkuna', 'PurchasingManager', 'Pending', 'Replacement', 'PR-2025-002', 3, 1, 0, NULL),
(NULL, 3, '2025-01-18', 'Large Cold Storage Unit', 'Sibongile Dlamini', 'PurchasingManager', 'Approved', 'Expansion', 'PR-2025-003', 2, 1, 1, '2025-01-19'),
(NULL, 4, '2025-02-01', 'Display Fridge', 'Tebogo Maseko', 'PurchasingManager', 'Approved', 'Retail', 'PR-2025-004', 6, 1, 0, NULL),
(NULL, 5, '2025-02-10', 'Beverage Freezer', 'Ayanda Ndlovu', 'PurchasingManager', 'Pending', 'Equipment', 'PR-2025-005', 4, 1, 0, NULL),
(NULL, 6, '2025-02-20', 'Ice Cream Display Unit', 'Samantha Molefe', 'PurchasingManager', 'Approved', 'Replacement', 'PR-2025-006', 3, 1, 1, '2025-02-21'),
(NULL, 7, '2025-03-05', 'Bar Cooler', 'Kabelo Khoza', 'PurchasingManager', 'Rejected', 'Equipment', 'PR-2025-007', 2, 1, 0, NULL),
(NULL, 8, '2025-03-15', 'Deep Freezer', 'Thabo Mthembu', 'PurchasingManager', 'Approved', 'Replacement', 'PR-2025-008', 5, 1, 1, '2025-03-16'),
(NULL, 9, '2025-03-25', 'Drinks Cooler', 'Amanda Sithole', 'PurchasingManager', 'Pending', 'Retail', 'PR-2025-009', 4, 1, 0, NULL),
(NULL, 10, '2025-04-02', 'Cold Beverage Fridge', 'Lucky Maduna', 'PurchasingManager', 'Approved', 'Expansion', 'PR-2025-010', 6, 1, 1, '2025-04-03');

INSERT INTO RequestsForQuotation (RFQNumber, PurchaseRequestID, CreatedDate, Deadline, Status, Description, RequiredQuantity)
VALUES
('RFQ-2025-001', 1, '2025-01-05', '2025-01-12', 'Draft', 'Quotation for Commercial Beverage Fridges', 5),
('RFQ-2025-002', 2, '2025-01-15', '2025-01-22', 'Draft', 'Quotation for Mini Coolers', 3),
('RFQ-2025-003', 3, '2025-02-02', '2025-02-09', 'Sent', 'Quotation for Large Cold Storage Units', 2),
('RFQ-2025-004', 4, '2025-02-11', '2025-02-18', 'Draft', 'Quotation for Display Fridges', 6),
('RFQ-2025-005', 5, '2025-02-25', '2025-03-04', 'Sent', 'Quotation for Beverage Freezers', 4),
('RFQ-2025-006', 6, '2025-03-10', '2025-03-17', 'Draft', 'Quotation for Ice Cream Display Units', 3),
('RFQ-2025-007', 7, '2025-03-20', '2025-03-27', 'Closed', 'Quotation for Bar Coolers', 2),
('RFQ-2025-008', 8, '2025-03-28', '2025-04-04', 'Draft', 'Quotation for Deep Freezers', 5),
('RFQ-2025-009', 9, '2025-04-05', '2025-04-12', 'Sent', 'Quotation for Drinks Coolers', 4),
('RFQ-2025-010', 10, '2025-04-10', '2025-04-17', 'Draft', 'Quotation for Cold Beverage Fridges', 6);


INSERT INTO Faults
(
    FaultDescription,
    FaultCode,
    Status,
    Priority,
    ScheduledDate,
    ReportDate,
    Notes,
    ApplianceType,
    InitialAssessment,
    EstimatedRepairTime,
    RequiredParts,
    IsUrgent,
    CreatedDate,
    UpdatedDate,
    Category,
    FridgeId,
    AssignedTechnicianId,
    CustomerId
)
VALUES
('Compressor not cooling properly', 'FC-001', 'Pending', 'High', '2025-10-25', '2025-10-15', 'Customer reported that fridge is not cooling well.', 'Refrigerator', 'Possible gas leak or compressor issue.', 4.5, 'Compressor unit, refrigerant gas', 1, GETDATE(), GETDATE(), 'Cooling Issue', 1, 2, 1),
('Door seal is worn out', 'FC-002', 'Diagnosing', 'Medium', '2025-10-26', '2025-10-16', 'Fridge door not closing properly.', 'Refrigerator', 'Seal appears damaged on the right side.', 2, 'New rubber door seal', 0, GETDATE(), GETDATE(), 'DoorSeal', 2, 2, 2),
('Fridge light not working', 'FC-003', 'Repairing', 'Low', '2025-10-28', '2025-10-18', 'Light bulb does not turn on.', 'Refrigerator', 'Likely a burnt bulb or wiring issue.', 1, 'Replacement bulb', 0, GETDATE(), GETDATE(), 'Electrical', 3, 2, 3),
('Noisy operation', 'FC-004', 'Testing', 'Medium', '2025-10-29', '2025-10-19', 'Strange noise from the back.', 'Freezer', 'Fan motor may be loose.', 3, 'Fan motor', 0, GETDATE(), GETDATE(), 'NoiseVibration', 4, 2, 11),
('Fridge leaking water', 'FC-005', 'Resolved', 'High', '2025-10-25', '2025-10-17', 'Water pooling at the bottom.', 'Refrigerator', 'Drain pipe clogged.', 2, 'Drain pipe replacement', 1, GETDATE(), GETDATE(), 'WaterLeak', 5, 2, 5),
('Power cable damaged', 'FC-006', 'Pending', 'High', '2025-10-27', '2025-10-18', 'Cable is frayed near plug.', 'Refrigerator', 'Electrical hazard risk.', 1, 'Power cable', 1, GETDATE(), GETDATE(), 'Electrical', 6, 2, 12),
('Thermostat malfunction', 'FC-007', 'InProgress', 'High', '2025-10-26', '2025-10-15', 'Temperature not stable.', 'Freezer', 'Thermostat sensor faulty.', 5, 'Thermostat sensor', 1, GETDATE(), GETDATE(), 'Thermostat', 7, 2, 7),
('Fridge not turning on', 'FC-008', 'Pending', 'Critical', '2025-10-24', '2025-10-18', 'Customer reports fridge dead.', 'Refrigerator', 'Possible fuse or compressor issue.', 6, 'Fuse, compressor', 1, GETDATE(), GETDATE(), 'PowerIssue', 8, 2, 8),
('Excessive frost build-up', 'FC-009', 'Diagnosing', 'Medium', '2025-10-27', '2025-10-19', 'Frost forming on inner walls.', 'Freezer', 'Defrost timer might be stuck.', 3.5, 'Defrost timer', 0, GETDATE(), GETDATE(), 'CoolingIssue', 9, 2, 9),
('Vibration during operation', 'FC-010', 'Testing', 'Low', '2025-10-30', '2025-10-19', 'Customer feels vibration.', 'Freezer', 'Compressor mount loose.', 1.5, 'Compressor mount bolts', 0, GETDATE(), GETDATE(), 'NoiseVibration', 10, 2, 10);

SELECT FridgeId FROM Fridge;
SELECT CustomerId FROM Customers;