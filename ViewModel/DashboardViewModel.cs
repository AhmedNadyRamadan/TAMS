namespace TASM.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalSessions { get; set; }
        public int CompletedSessions { get; set; }
        public int RemainingSessions { get; set; }
        public int TotalStudents { get; set; }
        public double AttendanceRate { get; set; }
        public DateOnly UpcomingSessionDate { get; set; }
        public List<SessionAttendanceGraph> SessionAttendanceGraphs { get; set; } = new List<SessionAttendanceGraph>();
    }

    public class SessionAttendanceGraph
    {
        public DateOnly SessionDate { get; set; }
        public double AttendanceRate { get; set; }
    }
}
