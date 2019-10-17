  SELECT c.Id,
		 c.FirstName + ' ' + c.LastName AS FullName,
		 c.Ucn
    FROM Colonists c
ORDER BY c.FirstName,
		 c.LastName,
		 c.Ucn