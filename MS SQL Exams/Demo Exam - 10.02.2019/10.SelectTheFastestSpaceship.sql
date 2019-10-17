  SELECT TOP 1
	     ss.Name,
	     sp.Name
    FROM Spaceships ss
    JOIN Journeys j
      ON j.SpaceshipId = ss.Id
    JOIN Spaceports sp
      ON sp.Id = j.DestinationSpaceportId
ORDER BY ss.LightSpeedRate DESC