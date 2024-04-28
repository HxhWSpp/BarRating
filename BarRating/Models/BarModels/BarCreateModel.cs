using System.ComponentModel.DataAnnotations;

namespace BarRating.Models.BarModels
{
    public class BarCreateModel
    {
        public int Id { get; set; }

        [MaxLength(64)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public string ImagePath { get; set; }


        public IFormFile Image { get; set; }
    }
}
