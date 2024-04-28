using Microsoft.AspNetCore.Identity;

namespace BarRating.Data.Entities
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            Reviews = new HashSet<Review>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}
