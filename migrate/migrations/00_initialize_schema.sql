CREATE TABLE IF NOT EXISTS show (
	id INTEGER PRIMARY KEY,
	date TEXT,
	name TEXT,
	event_link TEXT,
	location TEXT,
	venue TEXT
);
CREATE TABLE IF NOT EXISTS catalog (
	id INTEGER PRIMARY KEY,
	artist TEXT,
	title TEXT
);
