using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OpenCredentialPublisher.PublishingService.Data
{
    public class PublishRequest
    {
        public PublishRequest()
        {
            this.PublishState = PublishStates.Accepted;
            this.Pathway = Pathways.Publish1_0; 
            this.CreateTimestamp = DateTimeOffset.UtcNow;
            this.Files = new List<File>();
            this.AccessKeys = new List<AccessKey>();
            this.SigningKeys = new List<SigningKey>();
        }

        public PublishRequest(string requestId, string clientId, string requestIdentity, string filepath, string pathway, bool pushAfterPublish = false, string appUri = null, string pushUri = null) : this()
        {
            this.ClientId = clientId;
            this.RequestId = requestId;
            this.RequestIdentity = requestIdentity;
            this.Files.Add(File.CreateOriginal(filepath));
            this.PushAfterPublish = pushAfterPublish;
            this.PushUri = pushUri;
            this.AppUri = appUri;
            this.Pathway = pathway;
        }

        public void Deconstruct(out string requestId, out string clientId, out int revocationListId)
        {
            requestId = this.RequestId;
            clientId = this.ClientId;
            revocationListId = this.RevocationListId ?? 0;
        }

        [Key]
        [Column(TypeName = "nvarchar(128)"), Required]
        public string RequestId { get; set; }

        [Column(TypeName = "nvarchar(128)"), Required]
        public string ClientId { get; set; }

        [Column(TypeName = "nvarchar(128)"), Required]
        public string Pathway { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("RevocationList")]
        public int? RevocationListId { get; set; }
        [Column(TypeName = "nvarchar(32)")]
        public string RevocationReason { get; set; }

        [Column(TypeName = "nvarchar(128)"), Required]
        public string RequestIdentity { get; set; }

        [Column(TypeName = "datetimeoffset(7)"), Required]
        public DateTimeOffset CreateTimestamp { get; set; }

        public bool? ContainsPdf { get; set; }

        public bool PushAfterPublish { get; set; }
        [Column(TypeName = "nvarchar(256)")]
        public string PushUri { get; set; }
        [Column(TypeName = "nvarchar(256)")]
        public string AppUri { get; set; }

        [Column(TypeName = "datetimeoffset(7)")]
        public DateTimeOffset? PackageSignedTimestamp { get; set; }

        [Column(TypeName = "nvarchar(16)")]
        public string PublishState { get; set; }

        [Column(TypeName = "nvarchar(64)")]
        public string ProcessingState { get; set; }

        public virtual List<File> Files { get; set; }
        public virtual List<AccessKey> AccessKeys { get; set; }
        public virtual List<SigningKey> SigningKeys { get; set; }
        public virtual RevocationList RevocationList { get; set; }

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

        public static string AccessKeyUrl(string accessKeyUrl, string accessKey, string apiBaseUri, string scope, string endpoint, string method)
        {
            var url = HttpUtility.UrlEncode(apiBaseUri);
            var payload = HttpUtility.UrlEncode(JsonConvert.SerializeObject(new AccessKeyPayload { AccessKey = accessKey }));
            return string.Format(accessKeyUrl, url, scope, method, endpoint, payload);
        }
    }

    public class AccessKeyPayload
    {
        public string AccessKey { get; set; }
    }

   
}
