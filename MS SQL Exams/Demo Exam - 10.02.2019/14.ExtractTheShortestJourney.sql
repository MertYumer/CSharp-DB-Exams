  SELECT TOP 1
		 j.Id,
		 p.Name AS PlanetName,
		 s.Name AS SpaceportName,
		 j.Purpose AS JourneyPurpose
    FROM Journeys j
	JOIN Spaceports s
	  ON s.Id = j.DestinationSpaceportId
	JOIN Planets p
	  ON p.Id = s.PlanetId
ORDER BY DATEDIFF(SECOND, j.JourneyStart, j.JourneyEnd)