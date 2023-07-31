using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCredentialPublisher.PublishingService.Data
{
    public class File
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(64)"), Required]
        public string FileType { get; set; }

        [Column(TypeName = "nvarchar(128)"), Required]
        public string RequestId { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string ContainerId { get; set; }

        [Column(TypeName = "varchar(256)"), Required]
        public string FileName { get; set; }

        [Column(TypeName = "datetimeoffset(7)"), Required]
        public DateTimeOffset CreateTimestamp { get; set; }

        [ForeignKey("RequestId")]
        public virtual PublishRequest PublishRequest { get; set; }

        public static File CreateOriginal(string filename, string containerId = null)
        {
            var file = new File
            {
                FileType = ClrFileTypes.OriginalClr,
                FileName = filename,
                ContainerId = containerId,
                CreateTimestamp = DateTimeOffset.UtcNow
            };

            return file;
        }

        public static File CreateSigned(string filename, string containerId = null)
        {
            var file = new File
            {
                FileType = ClrFileTypes.SignedClr,
                FileName = filename,
                ContainerId = containerId,
                CreateTimestamp = DateTimeOffset.UtcNow
            };

            return file;
        }

        public static File CreateVCWrapped(string filename, string containerId = null)
        {
            var file = new File
            {
                FileType = ClrFileTypes.VCWrappedClr,
                FileName = filename,
                ContainerId = containerId,
                CreateTimestamp = DateTimeOffset.UtcNow
            };

            return file;
        }

        public static File CreateQrCodeImprintedPdf(string filename, string containerId = null)
        {
            var file = new File
            {
                FileType = ClrFileTypes.QrCodeImprintedPdf,
                FileName = filename,
                ContainerId = containerId,
                CreateTimestamp = DateTimeOffset.UtcNow
            };

            return file;
        }

        public static File CreateQrCodeImprintedClr(string filename, string containerId = null)
        {
            var file = new File
            {
                FileType = ClrFileTypes.QrCodeImprintedClr,
                FileName = filename,
                ContainerId = containerId,
                CreateTimestamp = DateTimeOffset.UtcNow
            };

            return file;
        }
    }

}
