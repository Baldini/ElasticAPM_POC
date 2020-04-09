namespace API1.Data
{
    public class Film
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int EpisodeId { get; set; }
        public string OpeningCrawl { get; set; }
        public string Director { get; set; }
        public string Producer { get; set; }
        public string ReleaseDate { get; set; }
    }
}
