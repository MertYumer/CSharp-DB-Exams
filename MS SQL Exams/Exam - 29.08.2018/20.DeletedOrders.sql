CREATE TABLE DeletedOrders(
       OrderId INT,
       ItemId INT, 
       ItemQuantity INT
)

CREATE TRIGGER tr_OrdersDelete ON OrderItems
 AFTER DELETE AS
INSERT INTO DeletedOrders(OrderId, ItemId, ItemQuantity)
	   (SELECT OrderId, ItemId, Quantity FROM deleted)