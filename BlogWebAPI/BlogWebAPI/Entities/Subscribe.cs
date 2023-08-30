namespace BlogWebAPI.Entities
{
    public class Subscribe
    {
        public int UserId { get; set; }
        public int SubscriberId { get; set; }

        public User User { get; set; } = null!;
        public User Subscriber { get; set; } = null!;
    }
}
