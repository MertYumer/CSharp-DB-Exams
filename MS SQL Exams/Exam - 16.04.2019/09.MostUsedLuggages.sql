 SELECT lt.Type,
	    COUNT(l.LuggageTypeId) AS [MostUsedLuggage]
   FROM LuggageTypes lt
   JOIN Luggages l
     ON l.LuggageTypeId = lt.Id
GROUP BY lt.Type
ORDER BY [MostUsedLuggage] DESC,
		 lt.Type