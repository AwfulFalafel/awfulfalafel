using Microsoft.Data.Sqlite;
namespace AwfulFalafelCommon;

public interface IDatabaseService
{
	public List<Show> GetUpcomingShows();
	public List<Show> GetPastShows();
	public List<Show> GetShows();
	public List<Song> GetSongs();
	public Song? GetSong(int id);
	public Song UpdateSong(int id, string artist, string title);
	public Song AddSong(string artist, string title);
	public void DeleteSong(int id);
	public Show? GetShow(int id);
	public Show UpdateShow(int id, string name, string eventLink, string location, string venue, DateTime date);
	public Show AddShow(string name, string eventLink, string location, string venue, DateTime date);
	public void DeleteShow(int id);
}

public class DatabaseService : IDatabaseService
{
	private readonly string DbPath;
	public DatabaseService(string dbPath)
	{
		DbPath = dbPath;
	}

	public List<Show> GetUpcomingShows()
	{
		var upcomingShowsList = new List<Show>();
		using (var connection = new SqliteConnection($"DataSource = {DbPath}"))
		{
			connection.Open();
			var upcomingShowsCommand = connection.CreateCommand();
			upcomingShowsCommand.CommandText = @"
			SELECT
				id
				, datetime(date) as date
				, name
				, event_link as eventLink
				, location
				, venue
			FROM
				show
			WHERE
				datetime(date) >= CURRENT_TIMESTAMP
			ORDER BY
				datetime(date) ASC";

			using var upcomingShowsReader = upcomingShowsCommand.ExecuteReader();
			while (upcomingShowsReader.Read())
			{
				upcomingShowsList.Add(
					GetShowFromRow(upcomingShowsReader)
				);
			}
		}

		return upcomingShowsList;
	}
	public List<Show> GetPastShows()
	{
		var pastShowsList = new List<Show>();
		using (var connection = new SqliteConnection($"DataSource = {DbPath}"))
		{
			connection.Open();
			var pastShowsCommand = connection.CreateCommand();
			pastShowsCommand.CommandText = @"
			SELECT
				id
				, datetime(date) as date
				, name
				, event_link as eventLink
				, location
				, venue
				FROM
					show
				WHERE
					datetime(date) < CURRENT_TIMESTAMP
				ORDER BY
					datetime(date) DESC";
			using var pastShowsReader = pastShowsCommand.ExecuteReader();
			while (pastShowsReader.Read())
			{
				pastShowsList.Add(GetShowFromRow(pastShowsReader));
			}
		}

		return pastShowsList;
	}
	public List<Show> GetShows()
	{
		var showsList = new List<Show>();
		using (var connection = new SqliteConnection($"DataSource = {DbPath}"))
		{
			connection.Open();
			var pastShowsCommand = connection.CreateCommand();
			pastShowsCommand.CommandText = @"
			SELECT
				id
				, datetime(date) as date
				, name
				, event_link as eventLink
				, location
				, venue
				FROM
					show
				ORDER BY
					datetime(date) DESC";
			using var pastShowsReader = pastShowsCommand.ExecuteReader();
			while (pastShowsReader.Read())
			{
				showsList.Add(GetShowFromRow(pastShowsReader));
			}
		}

		return showsList;
	}
	public List<Song> GetSongs()
	{
		var songList = new List<Song>();
		using (var connection = new SqliteConnection($"DataSource = {DbPath}"))
		{
			connection.Open();
			var command = connection.CreateCommand();
			command.CommandText = @"
				SELECT
					*
				FROM
					catalog
				ORDER BY
					artist ASC, title ASC;
			";
			using var reader = command.ExecuteReader();
			while (reader.Read())
			{
				songList.Add(GetSongFromRow(reader));
			}
		}
		return songList;
	}
	public Song? GetSong(int id)
	{
		Song song = default!;
		using var connection = new SqliteConnection($"DataSource = {DbPath}");
		connection.Open();
		var command = connection.CreateCommand();
		command.CommandText = @"
				SELECT
					*
				FROM
					catalog
				WHERE
					catalog.id = :id
			";
		command.Parameters.AddWithValue("id", id);
		using var reader = command.ExecuteReader();
		while (reader.Read())
		{
			song = GetSongFromRow(reader);
		}
		return song;
	}
	public Song UpdateSong(int id, string artist, string title)
	{
		using var connection = new SqliteConnection($"DataSource = {DbPath}");
		connection.Open();
		var command = connection.CreateCommand();
		command.CommandText = @"
			UPDATE
				catalog
			SET
				artist = :artist
				,	title = :title
			WHERE
				catalog.id = :id
			";
		command.Parameters.AddWithValue(":artist", artist);
		command.Parameters.AddWithValue(":title", title);
		command.Parameters.AddWithValue(":id", id);
		command.ExecuteNonQuery();
		var song = GetSong(id)!;
		return song;
	}
	public Song AddSong(string artist, string title)
	{
		Song song = default!;
		using var connection = new SqliteConnection($"DataSource = {DbPath}");
		connection.Open();
		var command = connection.CreateCommand();
		command.CommandText = @"
			INSERT INTO catalog (
				artist,
				title
			)
			VALUES (
				:artist
				,	:title
			)
			RETURNING *
			";
		command.Parameters.AddWithValue(":artist", artist);
		command.Parameters.AddWithValue(":title", title);
		var reader = command.ExecuteReader();
		while (reader.Read())
		{
			song = GetSongFromRow(reader);
		}
		return song;
		// return song;
	}
	public void DeleteSong(int id)
	{
		using var connection = new SqliteConnection($"DataSource = {DbPath}");
		connection.Open();
		var command = connection.CreateCommand();
		command.CommandText = @"
			DELETE FROM
				catalog
			WHERE
				catalog.id = :id
		";
		command.Parameters.AddWithValue(":id", id);
		command.ExecuteNonQuery();
		return;
	}
	public Show? GetShow(int id)
	{
		Show show = default!;
		using var connection = new SqliteConnection($"DataSource = {DbPath}");
		connection.Open();
		var command = connection.CreateCommand();
		command.CommandText = @"
				SELECT
					id
					,	datetime(date) as date
					,	name
					,	event_link as eventLink
					,	location
					,	venue
				FROM
					show
				WHERE
					show.id = :id
			";
		command.Parameters.AddWithValue("id", id);
		using var reader = command.ExecuteReader();
		while (reader.Read())
		{
			show = GetShowFromRow(reader);
		}
		return show;
	}
	public Show UpdateShow(int id, string name, string eventLink, string location, string venue, DateTime date)
	{
		using var connection = new SqliteConnection($"DataSource = {DbPath}");
		connection.Open();
		var command = connection.CreateCommand();
		command.CommandText = @"
			UPDATE
				show
			SET
				name = :name
				,	event_link = :eventLink
				,	location = :location
				,	venue = :venue
				,	date = :date
			WHERE
				show.id = :id
			";
		command.Parameters.AddWithValue(":id", id);
		command.Parameters.AddWithValue(":name", name);
		command.Parameters.AddWithValue(":eventLink", eventLink);
		command.Parameters.AddWithValue(":location", location);
		command.Parameters.AddWithValue(":venue", venue);
		command.Parameters.AddWithValue(":date", date);
		command.ExecuteNonQuery();
		var show = GetShow(id)!;
		return show;
	}
	public Show AddShow(string name, string eventLink, string location, string venue, DateTime date)
	{
		Show show = default!;
		using var connection = new SqliteConnection($"DataSource = {DbPath}");
		connection.Open();
		var command = connection.CreateCommand();
		command.CommandText = @"
			INSERT INTO show (
				name
				,	event_link
				,	location
				,	venue
				,	date
			)
			VALUES (
				:name
				,	:eventLink
				,	:location
				,	:venue
				,	:date
			)
			RETURNING *
			";
		command.Parameters.AddWithValue(":name", name);
		command.Parameters.AddWithValue(":eventLink", eventLink);
		command.Parameters.AddWithValue(":location", location);
		command.Parameters.AddWithValue(":venue", venue);
		command.Parameters.AddWithValue(":date", date);
		var reader = command.ExecuteReader();
		while (reader.Read())
		{
			show = GetShowFromRow(reader);
		}
		return show;
		// return song;
	}
	public void DeleteShow(int id)
	{
		using var connection = new SqliteConnection($"DataSource = {DbPath}");
		connection.Open();
		var command = connection.CreateCommand();
		command.CommandText = @"
			DELETE FROM
				show
			WHERE
				show.id = :id
		";
		command.Parameters.AddWithValue(":id", id);
		command.ExecuteNonQuery();
		return;
	}
	private static Song GetSongFromRow(SqliteDataReader row)
	{
		return new Song(
			row.GetInt16(0),
			row.GetString(1),
			row.GetString(2)
		);
	}
	private static Show GetShowFromRow(SqliteDataReader row)
	{
		int id = 0;
		string name = "";
		string eventLink = "";
		string location = "";
		string venue = "";
		if (!row.IsDBNull(0))
		{
			id = row.GetInt16(0);
		}
		DateTime date = DateTime.Parse(row.GetString(1));
		if (!row.IsDBNull(2))
		{
			name = row.GetString(2);
		}
		if (!row.IsDBNull(3))
		{
			eventLink = row.GetString(3);
		}
		if (!row.IsDBNull(4))
		{
			location = row.GetString(4);
		}
		if (!row.IsDBNull(5))
		{
			venue = row.GetString(5);
		}
		return new Show(
			Id: id,
			Date: date,
			Name: name,
			EventLink: eventLink,
			Location: location,
			Venue: venue
		);
	}
}