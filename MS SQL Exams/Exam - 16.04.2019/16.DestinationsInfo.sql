   SELECT f.Destination,
	 	  COUNT(t.FlightId) AS FilesCount
     FROM Flights f
LEFT JOIN Tickets t
	   ON t.FlightId = f.Id
 GROUP BY f.Destination
 ORDER BY FilesCount DESC,
		  f.Destination