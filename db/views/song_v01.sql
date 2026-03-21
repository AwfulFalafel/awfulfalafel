SELECT
	a."name" AS artist,
	catalog_entry.title
FROM
	catalog_entry
INNER JOIN artist a ON
	catalog_entry.artist_id = a.id
ORDER BY
	artist ASC,
	catalog_entry.title ASC