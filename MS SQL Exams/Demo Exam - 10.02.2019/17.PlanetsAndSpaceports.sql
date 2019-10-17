   SELECT p.Name,
		  COUNT(s.PlanetId) AS Count
     FROM Planets p
LEFT JOIN Spaceports s
	   ON s.PlanetId = p.Id
 GROUP BY p.Name
 ORDER BY COUNT(s.PlanetId) DESC,
		  p.Name