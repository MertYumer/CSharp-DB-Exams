CREATE TABLE ExcludedStudents
(
StudentId INT, 
StudentName VARCHAR(30)
)

CREATE TRIGGER tr_StudentsDelete ON Students
 AFTER DELETE AS
INSERT INTO ExcludedStudents(StudentId, StudentName)
	   (SELECT Id, FirstName + ' ' + LastName FROM deleted)