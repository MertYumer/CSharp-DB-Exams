  SELECT TOP 10
	     t.FirstName, 
	     t.LastName,
	     COUNT(st.TeacherId) AS StudentsCount
    FROM Teachers t
    JOIN StudentsTeachers st
      ON st.TeacherId = t.Id
GROUP BY t.FirstName,
		 t.LastName
ORDER BY StudentsCount DESC,
		 t.FirstName,
		 t.LastName