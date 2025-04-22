using System.Diagnostics;
using System.Security.Claims;
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
            // Get the current user's ID
            var userId = 1;

            // Get the lab associated with the current user
            var lab = _context.Labs
                .Include(l => l.Sessions)
                    .ThenInclude(s => s.SessionsStudents)
                .Include(l => l.Students)
                .FirstOrDefault(l => l.TaId == userId);
            if (lab == null)
            {
                return NotFound("Lab not found.");
            }
            // Get the lab's sessions
            var sessions = lab.Sessions.ToList();
            // Get the lab's students
            var students = lab.Students.ToList();
            // Get the lab's attendance records
            var attendanceRecords = _context.SessionsStudents
                .Include(ss => ss.Session)
                .Include(ss => ss.Student)
                .Where(ss => ss.Session.LabId == lab.Id)
                .ToList();

            // Get the lab's upcoming sessions
            var upcomingSessions = sessions
                .Where(s => s.Date > DateOnly.FromDateTime(DateTime.Now))
                .OrderBy(s => s.Date)
                .ToList();
            // Get the lab's completed sessions
            var completedSessions = sessions
                .Where(s => s.Date < DateOnly.FromDateTime(DateTime.Now))
                .OrderByDescending(s => s.Date)
                .ToList();
            // Get the lab's remaining sessions
            var remainingSessions = sessions
                .Where(s => s.Date >= DateOnly.FromDateTime(DateTime.Now))
                .OrderBy(s => s.Date)
                .ToList();

            // Get the lab's attendance rate
            var totalAttendance = attendanceRecords.Count;
            // Get the lab's attendance graphs
            var attendanceGraphs = sessions
                .Select(s => new SessionAttendanceGraph
                {
                    SessionDate = s.Date,
                    AttendanceRate = (double)attendanceRecords.Count(ss => ss.SessionId == s.Id) / students.Count * 100
                })
                .ToList();
            // Create the dashboard data
            var dashboardData = new DashboardViewModel
            {
                TotalSessions = sessions.Count,
                CompletedSessions = completedSessions.Count,
                RemainingSessions = remainingSessions.Count,
                TotalStudents = students.Count,
                AttendanceRate = totalAttendance > 0 ? (double)attendanceRecords.Count / totalAttendance * 100 : 0,
                UpcomingSessionDate = upcomingSessions.FirstOrDefault()?.Date ?? DateOnly.FromDateTime(DateTime.Now),
                SessionAttendanceGraphs = attendanceGraphs
            };

            // Check if the user is signed in
            // If the user is not signed in, redirect to the login page
            // If the user is signed in, return the dashboard view
            if (!_SignInManager.IsSignedIn(User))
            {
                return Redirect("/Identity/Account/Login");
            }
            // Return the dashboard view with the data
            return View(dashboardData);
        }

        // GET: /Home/Privacy
        public IActionResult Privacy()
        {
            return View();
        }
        // GET: /Home/Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
