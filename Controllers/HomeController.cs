using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TASM.Data;
using TASM.Models;
using TASM.ViewModels;

namespace TASM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TamsContext _context;
        private SignInManager<IdentityUser> _SignInManager;
        public HomeController(ILogger<HomeController> logger, TamsContext context, SignInManager<IdentityUser> SignInManager)
        {
            _logger = logger;
            _context = context;
            _SignInManager = SignInManager;
        }

        public IActionResult Index()
        {
            var currentLabId = 1; // This should be dynamically fetched based on the current lab or selection

            var lab = _context.Labs
                .Include(l => l.Sessions)
                .ThenInclude(s => s.SessionsStudents) // Ensure SessionsStudents is included
                .FirstOrDefault(l => l.Id == currentLabId);

            if (lab == null) return NotFound();

            var completedSessions = lab.Sessions.Count(s => s.Date <= DateOnly.FromDateTime(DateTime.Now));
            var remainingSessions = lab.Sessions.Count(s => s.Date > DateOnly.FromDateTime(DateTime.Now));

            var attendanceGraphs = lab.Sessions.Select(s => new SessionAttendanceGraph
            {
                SessionDate = s.Date,
                AttendanceRate = s.SessionsStudents.Any() ? s.SessionsStudents.Count(ss => ss.Attended) * 100.0 / s.SessionsStudents.Count() : 0
            }).ToList();

            var totalStudents = lab.Students.Count;
            var totalAttendanceRecords = lab.Sessions.Sum(s => s.SessionsStudents.Count);
            var totalAttendedRecords = lab.Sessions.Sum(s => s.SessionsStudents.Count(ss => ss.Attended));

            var upcomingSession = lab.Sessions
                .Where(s => s.Date > DateOnly.FromDateTime(DateTime.Now))
                .OrderBy(s => s.Date)
                .FirstOrDefault();

            var dashboardData = new DashboardViewModel
            {
                TotalSessions = lab.Sessions.Count,
                CompletedSessions = completedSessions,
                RemainingSessions = remainingSessions,
                TotalStudents = totalStudents,
                AttendanceRate = totalAttendanceRecords > 0
                    ? (totalAttendedRecords * 100.0) / totalAttendanceRecords
                    : 0,
                UpcomingSessionDate = upcomingSession?.Date ?? default(DateOnly),
                SessionAttendanceGraphs = attendanceGraphs
            };

            if (!_SignInManager.IsSignedIn(User))
            {
                return Redirect("/Identity/Account/Login");
            }

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
