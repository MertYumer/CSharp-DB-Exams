  SELECT DATEPART(DAY, o.DateTime) AS Day,
         CAST(AVG(oi.Quantity * i.Price) AS DECIMAL(15, 2)) AS [Total profit]
    FROM Orders o
    JOIN OrderItems oi
      ON oi.OrderId = o.Id
    JOIN Items i
      ON i.Id = oi.ItemId
GROUP BY DATEPART(DAY, o.DateTime)
ORDER BY Day