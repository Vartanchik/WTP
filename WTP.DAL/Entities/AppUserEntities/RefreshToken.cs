using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WTP.DAL.Entities.AppUserEntities
{
    public class RefreshToken : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }

        [Required]
        public DateTime ExpiryTime { get; set; }
    }
}
