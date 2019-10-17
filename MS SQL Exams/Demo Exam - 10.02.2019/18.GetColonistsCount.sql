 CREATE FUNCTION dbo.udf_GetColonistsCount(@PlanetName VARCHAR(30))
RETURNS INT
  BEGIN
		DECLARE @ColonistsCount INT = 
		(SELECT COUNT(tc.ColonistId) AS Count
		   FROM Planets p
		   JOIN Spaceports s
		     ON s.PlanetId = p.Id
		   JOIN Journeys j
		     ON j.DestinationSpaceportId = s.Id
		   JOIN TravelCards tc
		     ON tc.JourneyId = j.Id
		  WHERE p.Name = @PlanetName)

		RETURN @ColonistsCount
    END