using BarRating.Data.Entities;

namespace BarRating.Models.ReviewModels
{
    public class ReviewCreateModel
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public string UserId { get; set; }

        public int BarId { get; set; }
    }
}
