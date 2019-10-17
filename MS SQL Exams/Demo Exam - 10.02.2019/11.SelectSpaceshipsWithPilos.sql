  SELECT ss.Name,
		 ss.Manufacturer
    FROM Spaceships ss
	JOIN Journeys j
	  ON j.SpaceshipId = ss.Id
	JOIN TravelCards tc
	  ON tc.JourneyId = j.Id
	JOIN Colonists c
	  ON c.Id = tc.ColonistId
   WHERE DATEDIFF(year, c.BirthDate, '01/01/2019') < 30
     AND tc.JobDuringJourney = 'Pilot'
GROUP BY ss.Name,
		 ss.Manufacturer
ORDER BY ss.Name
