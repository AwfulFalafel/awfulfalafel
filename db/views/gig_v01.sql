SELECT
	c.date,
	c.social_link,
	venue.name AS venue_name,
	venue.address AS "location"
FROM
	schedule_entry c
INNER JOIN
	venue ON c.venue_id = venue.id
ORDER BY
	c.date DESC
