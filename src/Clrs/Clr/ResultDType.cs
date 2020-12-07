using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenCredentialsPublisher.Credentials.Clrs.Clr
{
    public class ResultDType
    {
        /// <summary>
        /// Unique IRI for the object.
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("achievedLevel", NullValueHandling = NullValueHandling.Ignore)]
        public string AchievedLevel { get; set; }
        [JsonProperty("alignments", NullValueHandling = NullValueHandling.Ignore)]
        public List<AlignmentDType> Alignments { get; set; }
        [JsonProperty("resultDescription", Required = Required.Always)]
        public string ResultDescription { get; set; }

        /// <summary>
        /// A grade or value representing the result of the performance, or demonstration, of the achievement.  For example, 'A' if the recipient received a grade of A in the class.
        /// </summary>
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }
    }

}
