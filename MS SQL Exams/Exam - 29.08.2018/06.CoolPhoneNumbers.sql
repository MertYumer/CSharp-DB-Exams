  SELECT e.FirstName + ' ' + e.LastName AS [Full Name],
		 e.Phone 
    FROM Employees e
   WHERE e.Phone LIKE '3%'
ORDER BY e.FirstName,
		 e.Phone