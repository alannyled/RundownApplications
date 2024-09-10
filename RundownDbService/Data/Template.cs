namespace RundownDbService.Data
{
    public class Template
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        public List<Video> VideoObjects { get; set; } = new List<Video>();
    }
}
