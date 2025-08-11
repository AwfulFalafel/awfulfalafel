using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace AwfulFalafel.Pages;

public class ScheduleModel : PageModel
{
	private readonly ILogger<ScheduleModel> _logger;
	private readonly DatabaseService databaseService;
	public List<Show>? UpcomingShows;
	public List<Show>? PastShows;
	public ScheduleModel(ILogger<ScheduleModel> logger, IConfiguration configuration)
	{
		_logger = logger;
		string? dbPath = configuration.GetConnectionString("Database");
		databaseService = new DatabaseService(dbPath);
	}

	public void OnGet()
	{
		PastShows = databaseService.GetPastShows();
		UpcomingShows = databaseService.GetUpcomingShows();
	}
}
