 SELECT b.[Teacher Full Name],
	    b.[Subject Name],
	    b.[Student Full Name],
	    b.Grade
   FROM
(SELECT a.[Teacher Full Name],
	    a.[Subject Name],
	    a.[Student Full Name],
	    a.Grade,
	    ROW_NUMBER() OVER (PARTITION BY a.[Teacher Full Name]  ORDER BY a.Grade DESC) as GradeRank
   FROM
	    (SELECT t.FirstName + ' ' + t.LastName AS [Teacher Full Name],
			    su.Name AS [Subject Name],
			    s.FirstName + ' ' + s.LastName AS [Student Full Name],
			    CAST(ROUND(AVG(ss.Grade), 2) AS DECIMAL(3,2)) AS Grade
		   FROM Teachers t
		   JOIN StudentsTeachers st
			 ON st.TeacherId = t.Id
		   JOIN Students s
			 ON s.Id = st.StudentId
		   JOIN StudentsSubjects ss
			 ON ss.StudentId = s.Id
		   JOIN Subjects su
			 ON su.Id = ss.SubjectId AND su.Id = t.SubjectId
	   GROUP BY t.FirstName,
			    t.LastName,
			    s.FirstName,
			    s.LastName,
			    su.Name
		) AS a
) AS b
WHERE b.GradeRank = 1
ORDER BY b.[Subject Name], 
		 b.[Teacher Full Name], 
		 b.Grade DESC