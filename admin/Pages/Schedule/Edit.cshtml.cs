using System.ComponentModel.DataAnnotations;
using AwfulFalafelCommon;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AwfulFalafelAdmin.Pages;

class ShowEditModel : PageModel
{
	private readonly ILogger<ShowEditModel> _logger;
	private readonly IDatabaseService _databaseService;
	public Show? Show { get; set; } = default!;
	[BindProperty(SupportsGet = true)]
	public int Id { get; set; }
	[BindProperty]
	public required string Name { get; set; }
	[BindProperty]
	public required string EventLink { get; set; }
	[BindProperty]
	public required string Location { get; set; }
	[BindProperty]
	public required string Venue { get; set; }

	[BindProperty]
	public DateTime Date { get; set; }

	public ShowEditModel(ILogger<ShowEditModel> logger, IDatabaseService databaseService)
	{
		_logger = logger;
		_databaseService = databaseService;
	}
	public void OnGet()
	{
		Show = _databaseService.GetShow(Id);
	}
	public void OnPost()
	{
		Show = _databaseService.UpdateShow(Id, Name, EventLink, Location, Venue, Date);
	}
}