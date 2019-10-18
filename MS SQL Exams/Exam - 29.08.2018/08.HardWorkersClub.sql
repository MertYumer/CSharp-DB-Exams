  SELECT e.FirstName,
		 e.LastName,
		 AVG(DATEDIFF(HOUR, s.CheckIn, s.CheckOut)) AS [Work Hours]
    FROM Employees e
	JOIN Shifts s
	  ON s.EmployeeId = e.Id
GROUP BY e.FirstName,
		 e.LastName,
		 e.Id
  HAVING AVG(DATEDIFF(HOUR, s.CheckIn, s.CheckOut)) > 7
ORDER BY [Work Hours] DESC,
		 e.Id