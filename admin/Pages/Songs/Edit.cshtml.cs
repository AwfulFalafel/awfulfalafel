using AwfulFalafelCommon;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AwfulFalafelAdmin.Pages;

class SongsEditModel : PageModel
{
	private readonly ILogger<SongsEditModel> _logger;
	private readonly IDatabaseService _databaseService;
	public Song? Song { get; set; } = default!;
	[BindProperty(SupportsGet = true )]
    public int Id { get; set; }
	[BindProperty]
	public required string Artist { get; set; }
	[BindProperty]
	public required string Title { get; set; }
	public SongsEditModel(ILogger<SongsEditModel> logger, IDatabaseService databaseService)
	{
		_logger = logger;
		_databaseService = databaseService;
	}
	public void OnGet()
	{
		Song = _databaseService.GetSong(Id);
	}
	public void OnPost()
	{
		Song = _databaseService.UpdateSong(Id, Artist, Title);
	}
}