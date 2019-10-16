  SELECT k.FirstName,
	     k.LastName,
	     k.Grade
    FROM (SELECT s.FirstName,
			     s.LastName,
			     ss.Grade,
			     ROW_NUMBER() OVER(PARTITION BY s.FirstName, s.LastName ORDER BY ss.Grade DESC) AS GradeRank
		    FROM Students s
		    JOIN StudentsSubjects ss
			  ON ss.StudentId = s.Id) AS k
   WHERE k.GradeRank = 2
ORDER BY k.FirstName,
		 k.LastName