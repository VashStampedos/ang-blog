namespace BlogWebAPI.Entities
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; } 
        public string Description { get; set; } 
        public string? Photo { get; set; }
        public int IdBlog { get; set; }
        public Blog Blog { get; set; } 
        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }



    }
}
