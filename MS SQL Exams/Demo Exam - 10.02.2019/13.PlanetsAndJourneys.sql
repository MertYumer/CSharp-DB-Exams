  SELECT p.Name AS PlanetName,
		 COUNT(*) AS JourneysCount
    FROM Planets p
	JOIN Spaceports s
	  ON s.PlanetId = p.Id
	JOIN Journeys j
	  ON j.DestinationSpaceportId = s.Id
GROUP BY p.Name
ORDER BY JourneysCount DESC,
		 p.Name