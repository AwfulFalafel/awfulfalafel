using AwfulFalafelCommon;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AwfulFalafelAdmin.Pages;

class SongsIndexModel : PageModel
{
	private readonly ILogger<SongsIndexModel> _logger;
	private readonly IDatabaseService _databaseService;
	public required List<Song> Songs = [];
	[BindProperty]
	public int Id { get; set; }
	[BindProperty]
	public required string Artist { get; set; }
	[BindProperty]
	public required string Title { get; set; }
	public SongsIndexModel(ILogger<SongsIndexModel> logger, IDatabaseService databaseService)
	{
		_logger = logger;
		_databaseService = databaseService;
	}

	public void OnGet()
	{
		Songs = _databaseService.GetSongs();
	}
	public IActionResult OnPost()
	{
		_databaseService.AddSong(Artist, Title);
		return RedirectToAction("Get");
	}
	public IActionResult OnPostDelete()
	{
		// var newSong = _databaseService.AddSong(Artist, Title);
		_databaseService.DeleteSong(Id);
		return RedirectToAction("Get");
	}
}
