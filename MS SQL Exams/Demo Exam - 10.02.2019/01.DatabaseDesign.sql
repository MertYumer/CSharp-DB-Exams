CREATE DATABASE ColonialJourney

CREATE TABLE Planets (
	   Id INT IDENTITY(1, 1) PRIMARY KEY,
	   Name VARCHAR(30) NOT NULL
)

CREATE TABLE Spaceports (
	   Id INT IDENTITY(1, 1) PRIMARY KEY,
	   Name VARCHAR(50) NOT NULL,
	   PlanetId INT NOT NULL REFERENCES Planets(Id)
)

CREATE TABLE Spaceships (
	   Id INT IDENTITY(1, 1) PRIMARY KEY,
	   Name VARCHAR(50) NOT NULL,
	   Manufacturer VARCHAR(30) NOT NULL,
	   LightSpeedRate INT DEFAULT 0
)

CREATE TABLE Colonists (
	   Id INT IDENTITY(1, 1) PRIMARY KEY,
	   FirstName VARCHAR(20) NOT NULL,
	   LastName VARCHAR(20) NOT NULL,
	   Ucn VARCHAR(10) NOT NULL UNIQUE,
	   BirthDate DATETIME2 NOT NULL
)

CREATE TABLE Journeys (
	   Id INT IDENTITY(1, 1) PRIMARY KEY,
	   JourneyStart DATETIME2 NOT NULL,
	   JourneyEnd DATETIME2 NOT NULL,
	   Purpose VARCHAR(11) CHECK(Purpose IN('Medical', 'Technical', 'Educational', 'Military')),
	   DestinationSpaceportId INT NOT NULL REFERENCES Spaceports(Id),
	   SpaceshipId INT NOT NULL REFERENCES Spaceships(Id)
)

CREATE TABLE TravelCards (
	   Id INT IDENTITY(1, 1) PRIMARY KEY,
	   CardNumber CHAR(10) NOT NULL UNIQUE,
	   JobDuringJourney VARCHAR(8) CHECK(JobDuringJourney IN('Pilot', 'Engineer', 'Trooper', 'Cleaner', 'Cook')),
	   ColonistId INT NOT NULL REFERENCES Colonists(Id),
	   JourneyId INT NOT NULL REFERENCES Journeys(Id)
)