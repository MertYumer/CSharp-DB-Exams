  SELECT TOP 10
		 e.FirstName + ' ' + e.LastName AS [Full Name],
		 SUM(oi.Quantity * i.Price) AS [Total Price],
		 SUM(oi.Quantity) AS Items
    FROM Employees e
	JOIN Orders o
	  ON o.EmployeeId = e.Id
	JOIN OrderItems oi
	  ON oi.OrderId = o.Id
	JOIN Items i
	  ON i.Id = oi.ItemId
   WHERE o.DateTime < '2018-06-15'
GROUP BY e.FirstName,
		 e.LastName
ORDER BY [Total Price] DESC,
		 Items DESC