namespace ReferModels
{
    public class App
    {
        public int Id { get; set; }
        public string AppStoreLink { get; set; }
        public string Name { get; set; }
        public string ImageLink { get; set; }
        public string Description { get; set; }
        public AppPlatform Platform { get; set; }
        public int DeveloperId { get; set; }
    }
}