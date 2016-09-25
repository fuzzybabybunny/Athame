namespace Athame.CommonModel
{
    public class Track
    {
        public string Id { get; set; }
        public string Artist { get; set; }
        public Album Album { get; set; }
        public string Title { get; set; }
        public int TrackNumber { get; set; }
        public int DiscNumber { get; set; }
        public string Genre { get; set; }
        public string Composer { get; set; }
        public int Year { get; set; }
        public string FileExtension { get; set; }
    }
}
