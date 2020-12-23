using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCredentialPublisher.PublishingService.Data
{
    public class AccessKey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(128)"), Required]
        public string RequestId { get; set; }

        [Column(TypeName = "nvarchar(128)"), Required]
        public string Key { get; set; }

        [Column(TypeName = "datetimeoffset(7)"), Required]
        public DateTimeOffset CreateTimestamp { get; set; }

        [Required]
        public bool Expired { get; set; }

        [ForeignKey("RequestId")]
        public virtual PublishRequest PublishRequest { get; set; }

        public static AccessKey Create()
        {
            var key = new AccessKey()
            {
                CreateTimestamp = DateTimeOffset.UtcNow,
                Key = Guid.NewGuid().ToString("d")
            };

            return key;
        }

       
    }

}
