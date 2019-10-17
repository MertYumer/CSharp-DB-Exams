  SELECT c.Id,
		 c.FirstName + ' ' + c.LastName AS FullName
    FROM Colonists c
	JOIN TravelCards tc
	  ON tc.ColonistId = c.Id
   WHERE tc.JobDuringJourney = 'Pilot'
ORDER BY c.Id