  SELECT p.PassportId AS [Passport Id],
	     p.Address
    FROM Passengers p
   WHERE p.Id NOT IN (SELECT l.PassengerId
					    FROM Luggages l)
ORDER BY p.PassportId,
		 p.Address