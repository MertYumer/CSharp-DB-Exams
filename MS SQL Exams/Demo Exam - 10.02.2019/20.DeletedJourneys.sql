CREATE TABLE DeletedJourneys (
	   Id INT,
	   JourneyStart DATETIME2,
	   JourneyEnd DATETIME2,
	   Purpose VARCHAR(11),
	   DestinationSpaceportId INT,
	   SpaceshipId INT
)

CREATE TRIGGER tr_JourneysDelete ON Journeys
 AFTER DELETE AS
INSERT INTO DeletedJourneys(Id, JourneyStart, JourneyEnd, Purpose, DestinationSpaceportId, SpaceshipId)
	   (SELECT Id, JourneyStart, JourneyEnd, Purpose, DestinationSpaceportId, SpaceshipId FROM deleted)