namespace DemoCards
{
    public class Country
    {
        public Country(string name, string subtitle, string description, string imageUrl)
        {
            Name = name;
            Subtitle = subtitle;
            Description = description;
            ImageUrl = imageUrl;
        }

        public string Name { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}