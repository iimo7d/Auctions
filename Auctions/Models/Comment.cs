using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Auctions.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Content { get; set; }


        [Required]
        public string? IdentityUserId { get; set; }

        [ForeignKey(nameof(IdentityUserId))]
        public IdentityUser? User { get; set; }
        public int? ListingId { get; set; }

        [ForeignKey(nameof(ListingId))]
        public Listing? listing { get; set; }
    }
}

