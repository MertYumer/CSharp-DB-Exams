  SELECT e.Id,
		 e.FirstName + ' ' + e.LastName AS [Full Name]
    FROM Employees e
	JOIN Shifts s
	  ON s.EmployeeId = e.Id
   WHERE DATEDIFF(HOUR, s.CheckIn, s.CheckOut) < 4
GROUP BY e.Id,
		 e.FirstName,
		 e.LastName
ORDER BY e.Id