namespace RundownDbService.Data
{
    public class Video
    {
        public int VideoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly Duration { get; set; }

        // Navigation property til de relaterede Rundowns
        public List<Rundown>? Rundowns { get; set; }
    }
}
