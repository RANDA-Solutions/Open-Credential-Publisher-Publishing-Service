using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCredentialPublisher.PublishingService.Data
{
    public class ClrPublishLog
    {
        public ClrPublishLog()
        {
            this.Timestamp = DateTime.UtcNow;
        }

        public ClrPublishLog(string clientId, string requestId, string action, string message) : this()
        {
            this.ClientId = clientId;
            this.RequestId = requestId;
            this.Action = action;
            this.Message = message;
        }

        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(128)")]
        public string RequestId { get; set; }

        [Column(TypeName = "nvarchar(128)")]
        public string ClientId { get; set; }

        [Column(TypeName = "nvarchar(32)"), Required]
        public string Action { get; set; }

        [Column(TypeName = "nvarchar(max)"), Required]
        public string Message { get; set; }

        [Column(TypeName = "datetimeoffset(7)"), Required]
        public DateTimeOffset Timestamp { get; set; }
    }

}
