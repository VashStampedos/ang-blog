using BlogWebAPI.Entities;

namespace BlogWebAPI.Models
{
    public class BlogModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int IdUser { get; set; }
        public UserModel User { get; set; } = null!;
        public ICollection<ArticleModel> Articles { get; set; } = null!;
    }
}
