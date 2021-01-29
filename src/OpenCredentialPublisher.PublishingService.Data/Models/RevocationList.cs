using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OpenCredentialPublisher.PublishingService.Data
{
    [Table("RevocationLists")]
    public class RevocationList
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(32)"), Required]
        public string PublicId { get; set; }
    }
}
