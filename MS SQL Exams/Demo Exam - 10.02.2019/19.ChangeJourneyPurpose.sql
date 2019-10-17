 CREATE PROC usp_ChangeJourneyPurpose(@JourneyId INT, @NewPurpose VARCHAR(30))
     AS
DECLARE @Id INT = (
 SELECT j.Id
   FROM Journeys j
  WHERE j.Id = @JourneyId
)

	 IF (@Id IS NULL)
  BEGIN
		RAISERROR('The journey does not exist!', 16, 1)
		RETURN
    END

DECLARE @Purpose VARCHAR(30) = (
 SELECT j.Purpose
   FROM Journeys j
  WHERE j.Id = @JourneyId
)

	 IF (@Purpose = @NewPurpose)
  BEGIN
		RAISERROR('You cannot change the purpose!', 16, 1)
		RETURN
    END

 UPDATE Journeys
    SET Purpose = @NewPurpose
  WHERE Id = @JourneyId