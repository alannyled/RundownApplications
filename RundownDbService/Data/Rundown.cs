namespace RundownDbService.Data
{
    public class Rundown
    {
        public int RundownId { get; set; } 
        public string RundownName { get; set; }
        public string RundownType { get; set; }
        public DateTime RundownDate { get; set; }
        public DateTime ArchivedDate { get; set; }
        // Foreign key for ControlRoom
        public int ControlRoomId { get; set; }

        // Reference navigation property for the related ControlRoom
        public ControlRoom ControlRoom { get; set; }

        public List<Video> VideoObjects { get; set; } = [];

       
    }
}
