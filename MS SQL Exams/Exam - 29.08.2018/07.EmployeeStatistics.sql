  SELECT e.FirstName,
		 e.LastName,
		 COUNT(o.EmployeeId) AS Count
    FROM Employees e
	JOIN Orders o
	  ON o.EmployeeId = e.Id
GROUP BY e.FirstName,
		 e.LastName
ORDER BY Count DESC,
		 e.FirstName