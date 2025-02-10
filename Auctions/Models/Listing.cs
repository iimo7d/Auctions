using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auctions.Models
{
    public class Listing
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
        public string ImagePath { get; set; }

        [Required]
        public bool IsSold { get; set; } = false;

        [Required]
        public string? IdentityUserId {  get; set; }

        [ForeignKey(nameof(IdentityUserId))]
        public IdentityUser? User { get; set; }

        public List<Bid>? Bids { get; set; }
        public List<Comment>? Comments { get; set; }

    }
}
