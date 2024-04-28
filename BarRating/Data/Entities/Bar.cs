using System.ComponentModel.DataAnnotations;

namespace BarRating.Data.Entities
{
    public class Bar
    {
        public int Id { get; set; }

        [MaxLength(64)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public string ImagePath { get; set; }

        public virtual ICollection<Review>? Reviews { get; set; }
    }
}
