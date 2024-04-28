namespace BarRating.Data.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public virtual AppUser? User { get; set; }

        public virtual Bar? Bar { get; set; }
    }
}
