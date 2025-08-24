using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace AwfulFalafel.Pages;

public class ScheduleModel : PageModel
{
	private readonly ILogger<ScheduleModel> _logger;
	private readonly IDatabaseService _databaseService;
	public List<Show>? UpcomingShows;
	public List<Show>? PastShows;
	public ScheduleModel(ILogger<ScheduleModel> logger, IDatabaseService databaseService)
	{
		_logger = logger;
		_databaseService = databaseService;
	}

	public void OnGet()
	{
		PastShows = _databaseService.GetPastShows();
		UpcomingShows = _databaseService.GetUpcomingShows();
	}
}
