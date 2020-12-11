using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OpenCredentialsPublisher.PublishingService.Data
{
    public class PublishRequest
    {
        public PublishRequest()
        {
            this.PublishState = PublishStates.Accepted;
            this.CreateTimestamp = DateTimeOffset.UtcNow;
            this.Files = new List<File>();
            this.AccessKeys = new List<AccessKey>();
            this.SigningKeys = new List<SigningKey>();
        }

        public PublishRequest(string requestId, string clientId, string requestIdentity, string filepath) : this()
        {
            this.ClientId = clientId;
            this.RequestId = requestId;
            this.RequestIdentity = requestIdentity;
            this.Files.Add(File.CreateOriginal(filepath));
        }

        [Key]
        [Column(TypeName = "nvarchar(128)"), Required]
        public string RequestId { get; set; }

        [Column(TypeName = "nvarchar(128)"), Required]
        public string ClientId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(128)"), Required]
        public string RequestIdentity { get; set; }

        [Column(TypeName = "datetimeoffset(7)"), Required]
        public DateTimeOffset CreateTimestamp { get; set; }

        public bool? ContainsPdf { get; set; }

        [Column(TypeName = "datetimeoffset(7)")]
        public DateTimeOffset? PackageSignedTimestamp { get; set; }

        [Column(TypeName = "nvarchar(16)")]
        public string PublishState { get; set; }

        [Column(TypeName = "nvarchar(64)")]
        public string ProcessingState { get; set; }

        public virtual List<File> Files { get; set; }
        public virtual List<AccessKey> AccessKeys { get; set; }
        public virtual List<SigningKey> SigningKeys { get; set; }

    }


    public static class PublishRequestExtensions
    {
        public static File GetOriginalClr(this PublishRequest request)
        {
            return request?.Files.Where(f => f.FileType == ClrFileTypes.OriginalClr).OrderByDescending(f => f.CreateTimestamp).FirstOrDefault();
        }

        public static File GetSignedClr(this PublishRequest request)
        {
            return request?.Files.Where(f => f.FileType == ClrFileTypes.SignedClr).OrderByDescending(f => f.CreateTimestamp).FirstOrDefault();
        }

        public static File GetQrCodeImprintedClr(this PublishRequest request)
        {
            return request?.Files.Where(f => f.FileType == ClrFileTypes.QrCodeImprintedClr).OrderByDescending(f => f.CreateTimestamp).FirstOrDefault();
        }

        public static File GetVcWrappedClr(this PublishRequest request)
        {
            return request?.Files.Where(f => f.FileType == ClrFileTypes.VCWrappedClr).OrderByDescending(f => f.CreateTimestamp).FirstOrDefault();
        }

        public static AccessKey LatestAccessKey(this PublishRequest request)
        {
            return request?.AccessKeys.Where(k => !k.Expired).OrderByDescending(f => f.CreateTimestamp).FirstOrDefault();
        }

        public static string AccessKeyUrl(string accessKeyUrl, string accessKey, string apiBaseUri)
        {
            return string.Format(accessKeyUrl, HttpUtility.UrlEncode(accessKey), HttpUtility.UrlEncode(apiBaseUri));
        }
    }

   
}
