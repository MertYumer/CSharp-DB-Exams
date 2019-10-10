   SELECT p.FirstName AS [First Name],
	      p.LastName AS [Last Name],
		  COUNT(t.PassengerId) AS [Total Trips]
     FROM Passengers p
LEFT JOIN Tickets t
	   ON t.PassengerId = p.Id
 GROUP BY p.FirstName,
 		  p.LastName
 ORDER BY [Total Trips] DESC,
 		  p.FirstName,
 		  p.LastName