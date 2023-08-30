namespace BlogWebAPI.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public int IdUser { get; set; }
        public int IdArticle { get; set; }

        public User User { get; set; } = null!;
        public Article Article { get; set; } = null!;
    }
}
