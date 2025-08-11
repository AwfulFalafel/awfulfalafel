namespace AwfulFalafel.Pages;

using Microsoft.Data.Sqlite;

public class DatabaseService : IDisposable
{
	private readonly SqliteConnection Connection;
	public void Dispose()
	{
		GC.SuppressFinalize(this);
		Connection.Close();
	}
	public DatabaseService(string dbPath)
	{
		// Console.WriteLine(Environment.CurrentDirectory);
		// Console.WriteLine("File exists: {0}", File.Exists(dbPath).ToString());
		// Console.WriteLine("Resolved path: {0}", Path.GetFullPath(dbPath));
		Connection = new($"DataSource = {dbPath}");
		Connection.Open();
	}

	public List<Show> GetUpcomingShows()
	{
		var upcomingShowsList = new List<Show>();
		var upcomingShowsCommand = Connection.CreateCommand();
		upcomingShowsCommand.CommandText = @"
			SELECT
				datetime(date) as date
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

		return upcomingShowsList;
	}
	public List<Show> GetPastShows()
	{
		var pastShowsList = new List<Show>();
		var pastShowsCommand = Connection.CreateCommand();
		pastShowsCommand.CommandText = @"
			SELECT
				datetime(date) as date
				,	name
				,	event_link as eventLink
				,	location
				,	venue
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

		return pastShowsList;
	}

	public List<Song> GetSongs()
	{
		var songList = new List<Song>();
		var command = Connection.CreateCommand();
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
			songList.Add(new Song(Artist: reader.GetString(1), Title: reader.GetString(2)));
		}
		return songList;
	}
	private static Show GetShowFromRow(SqliteDataReader row)
	{
		string name = "";
		if (!row.IsDBNull(1))
		{
			name = row.GetString(1);
		}
		string eventLink = "";
		if (!row.IsDBNull(2))
		{
			eventLink = row.GetString(2);
		}
		string location = "";
		if (!row.IsDBNull(3))
		{
			location = row.GetString(3);
		}
		string venue = "";
		if (!row.IsDBNull(4))
		{
			venue = row.GetString(4);
		}
		return new Show(
			Date: DateTime.Parse(row.GetString(0)),
			Name: name,
			EventLink: eventLink,
			Location: location,
			Venue: venue
		);
	}
}