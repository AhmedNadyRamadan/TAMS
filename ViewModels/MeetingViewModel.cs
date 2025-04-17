namespace TASM.ViewModels
{
    public class MeetingViewModel
    {
        public string Topic { get; set; }
        public int Duration { get; set; }
        public string Timezone { get; set; }
        public DateTime StartTime { get; set; }
        public bool HostVideo { get; set; }
        public bool ParticipantVideo { get; set; }
    }


}
