using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialsPublisher.Credentials.Clrs.Clr
{
    public class RubricCriterionLevelDType
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("alignments", NullValueHandling = NullValueHandling.Ignore)]
        public List<AlignmentDType> Alignments { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty("level", NullValueHandling = NullValueHandling.Ignore)]
        public string Level { get; set; }
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }
        [JsonProperty("points", NullValueHandling = NullValueHandling.Ignore)]
        public string Points { get; set; }
    }
}
