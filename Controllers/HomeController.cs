using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TASM.Data;
using TASM.Models;
using TASM.ViewModels;

namespace TASM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TamsContext _context;

        public HomeController(ILogger<HomeController> logger, TamsContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var dashboardData = new DashboardViewModel
            {
                LabsThisWeek = _context.Labs.Count(), 
                UpcomingSessions = _context.Sessions.Count(s => s.Date > DateOnly.FromDateTime(DateTime.Now)),
                AttendanceRate = 85, 
                NextMeeting = "Monday, April 21st at 10:00 AM" 
            };

            return View(dashboardData);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
