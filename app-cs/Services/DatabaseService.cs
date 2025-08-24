namespace AwfulFalafel.Pages;

using Microsoft.Data.Sqlite;

public interface IDatabaseService
{
	public List<Show> GetUpcomingShows();
	public List<Show> GetPastShows();
	public List<Song> GetSongs();
}

public class DatabaseService : IDatabaseService
{
	private readonly string DbPath;
	public DatabaseService(IConfiguration configuration)
	{
		DbPath = configuration.GetConnectionString("Database")!;
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
		}

		return pastShowsList;
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
				songList.Add(new Song(Artist: reader.GetString(1), Title: reader.GetString(2)));
			}
		}
		return songList;
	}
	private static Show GetShowFromRow(SqliteDataReader row)
	{
		string name = "";
		string eventLink = "";
		string location = "";
		string venue = "";
		if (!row.IsDBNull(1))
		{
			name = row.GetString(1);
		}
		if (!row.IsDBNull(2))
		{
			eventLink = row.GetString(2);
		}
		if (!row.IsDBNull(3))
		{
			location = row.GetString(3);
		}
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