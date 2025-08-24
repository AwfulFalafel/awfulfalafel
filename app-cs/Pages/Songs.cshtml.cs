using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace AwfulFalafel.Pages;

public class SongsModel : PageModel
{
	private readonly ILogger<SongsModel> _logger;
	private readonly IDatabaseService _databaseService;
	public List<Song>? SongList;

	public SongsModel(ILogger<SongsModel> logger, IDatabaseService databaseService)
	{
		_logger = logger;
		_databaseService = databaseService;
	}

	public void OnGet()
	{

		SongList = _databaseService.GetSongs();
	}
}
