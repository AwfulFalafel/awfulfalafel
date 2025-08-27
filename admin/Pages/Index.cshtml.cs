using AwfulFalafelCommon;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AwfulFalafelAdmin.Pages;

public class IndexModel : PageModel
{
	private readonly ILogger<IndexModel> _logger;
	private readonly IDatabaseService _databaseService;
	public required List<Song> Songs = [];

	public IndexModel(ILogger<IndexModel> logger, IDatabaseService databaseService)
	{
		_logger = logger;
		_databaseService = databaseService;
	}

	public void OnGet()
	{
		Songs = _databaseService.GetSongs();
	}
}
