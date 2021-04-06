﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace OpenCredentialPublisher.Credentials.Clrs.Clr
{
    public class EvidenceDType
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("artifacts", NullValueHandling = NullValueHandling.Ignore)]
        public List<ArtifactDType> Artifacts { get; set; }
        [JsonProperty("audience", NullValueHandling = NullValueHandling.Ignore)]
        public string Audience { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty("genre", NullValueHandling = NullValueHandling.Ignore)]
        public string Genre { get; set; }
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }
        /// <summary>
        /// Markdown formatted string
        /// </summary>
        [JsonProperty("narrative", NullValueHandling = NullValueHandling.Ignore)]
        public string Narrative { get; set; }
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }

}
