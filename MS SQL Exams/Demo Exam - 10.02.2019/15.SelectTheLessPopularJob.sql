  SELECT TOP 1
		 j.Id AS JourneyId,
		 tc.JobDuringJourney AS JobName
    FROM Journeys j
	JOIN TravelCards tc
	  ON tc.JourneyId = j.Id
	JOIN Colonists c
	  ON c.Id = tc.ColonistId
GROUP BY j.Id,
		 tc.JobDuringJourney,
		 j.JourneyStart,
		 j.JourneyEnd
ORDER BY DATEDIFF(SECOND, j.JourneyStart, j.JourneyEnd) DESC,
		 COUNT(tc.ColonistId)