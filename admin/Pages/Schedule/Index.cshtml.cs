using System.ComponentModel.DataAnnotations;
using System.Data;
using AwfulFalafelCommon;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AwfulFalafelAdmin.Pages;

class ShowIndexModel : PageModel
{
	private readonly ILogger<ShowIndexModel> _logger;
	private readonly IDatabaseService _databaseService;
	public required List<Show> Shows = [];
	[BindProperty]
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
	public required DateTime Date { get; set; } = DateTime.Now;
	public ShowIndexModel(ILogger<ShowIndexModel> logger, IDatabaseService databaseService)
	{
		_logger = logger;
		_databaseService = databaseService;
	}
	public void OnGet()
	{
		Shows = _databaseService.GetShows();
	}
	public IActionResult OnPost()
	{
		_databaseService.AddShow(Name, EventLink, Location, Venue, Date);
		return RedirectToAction("Get");
	}
	public IActionResult OnPostDelete()
	{
		_databaseService.DeleteShow(Id);
		return RedirectToAction("Get");
	}
}
