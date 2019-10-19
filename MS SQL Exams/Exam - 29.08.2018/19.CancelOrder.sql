 CREATE PROC usp_CancelOrder(@OrderId INT, @CancelDate DATETIME2)
     AS 
DECLARE @id INT = (SELECT o.Id
					 FROM Orders o
					WHERE o.Id = @OrderId)

     IF (@id IS NULL)
  BEGIN
	    RAISERROR('The order does not exist!', 16, 1)
        RETURN
    END

DECLARE @OrderDate DATETIME2 = (SELECT o.DateTime
					        FROM Orders o
					       WHERE o.Id = @OrderId)

     IF (DATEDIFF(DAY, @OrderDate, @CancelDate) > 3)
  BEGIN
	    RAISERROR('You cannot cancel the order!', 16, 1)
        RETURN
    END

 DELETE 
   FROM OrderItems
  WHERE OrderId = @OrderId

 DELETE 
   FROM Orders
  WHERE Id = @OrderId