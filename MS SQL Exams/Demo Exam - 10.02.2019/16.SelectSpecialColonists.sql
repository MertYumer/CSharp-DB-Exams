SELECT k.JobDuringJourney,
	   k.FullName,
	   k.ColonistRank
  FROM (SELECT tc.JobDuringJourney,
			   c.FirstName + ' ' + c.LastName AS FullName,
			   DENSE_RANK() OVER (PARTITION BY tc.JobDuringJourney ORDER BY c.BirthDate) AS ColonistRank
		  FROM TravelCards tc
		  JOIN Colonists c
			ON c.Id = tc.ColonistId
		  JOIN Journeys j
			ON j.Id = tc.JourneyId) AS k
 WHERE k.ColonistRank = 2