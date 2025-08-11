using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace AwfulFalafel.Pages;

public class SongsModel : PageModel
{
	private readonly ILogger<SongsModel> _logger;
	private readonly DatabaseService databaseService;
	public List<Song>? SongList;

	public SongsModel(ILogger<SongsModel> logger, IConfiguration configuration)
	{
		_logger = logger;
		string? dbPath = configuration.GetConnectionString("Database");
		databaseService = new DatabaseService(dbPath);
	}

	public void OnGet()
	{

		SongList = databaseService.GetSongs();
	}
}
