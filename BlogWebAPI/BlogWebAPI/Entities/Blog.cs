namespace BlogWebAPI.Entities
{
    public class Blog
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int IdUser { get; set; }
        public User? User { get; set; }
        public ICollection<Article>? Articles { get; set; } = null!;
    }
}
