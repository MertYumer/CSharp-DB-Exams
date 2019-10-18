  SELECT TOP 1
		 oi.OrderId,
	     SUM(oi.Quantity * i.Price) AS TotalPrice
    FROM OrderItems oi
    JOIN Items i
      ON i.Id = oi.ItemId
GROUP BY oi.OrderId
ORDER BY TotalPrice DESC