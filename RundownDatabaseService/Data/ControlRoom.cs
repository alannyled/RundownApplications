namespace RundownDbService.Data
{
    public class ControlRoom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        public List<Hardware>? Hardwares { get; set; } = new List<Hardware>();
        public List<Rundown>? Rundowns { get; set; } = new List<Rundown>();
    }
}
