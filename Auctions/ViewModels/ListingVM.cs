using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Auctions.ViewModels
{
    public class ListingVM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        [Required]
        public bool IsSold { get; set; } = false;

        [Required]
        public string IdentityUserId { get; set; }

        [ForeignKey(nameof(IdentityUserId))]
        public IdentityUser? User { get; set; }
    }
}
