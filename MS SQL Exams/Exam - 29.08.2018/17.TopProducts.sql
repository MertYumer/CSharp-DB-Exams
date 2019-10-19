   SELECT i.Name AS Item,
          c.Name AS Category,
		  SUM(oi.Quantity) AS Count,
		  SUM(oi.Quantity * i.Price) AS TotalPrice
     FROM Items i
	 JOIN Categories c
	   ON c.Id = i.CategoryId
LEFT JOIN OrderItems oi
	   ON oi.ItemId = i.Id
 GROUP BY i.Name,
          c.Name
 ORDER BY TotalPrice DESC,
          Count DESC