  SELECT j.Id,
	     FORMAT (j.JourneyStart, 'dd/MM/yyyy', 'en-gb') AS JourneyStart,
		 FORMAT (j.JourneyEnd, 'dd/MM/yyyy', 'en-gb') AS JourneyEnd
    FROM Journeys j
   WHERE j.Purpose = 'Military'
ORDER BY j.JourneyStart