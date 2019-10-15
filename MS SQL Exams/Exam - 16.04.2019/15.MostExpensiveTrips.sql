  SELECT k.FirstName,
	     k.LastName,
	     k.Destination,
	     k.Price  
	FROM (SELECT p.FirstName,
				 p.LastName,
				 f.Destination,
				 t.Price,
				 DENSE_RANK() OVER(PARTITION BY p.FirstName, p.LastName ORDER BY t.Price DESC) 
				 AS PriceRank
			FROM Passengers p
			JOIN Tickets t
			  ON t.PassengerId = p.Id
			JOIN Flights f
			  ON f.Id = t.FlightId) 
	  AS k
   WHERE k.PriceRank = 1
ORDER BY k.Price DESC,
		 k.FirstName,
		 k.LastName,
		 k.Destination