﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace OpenCredentialPublisher.Credentials.Clrs.Clr
{
    public class ResultDescriptionDType
    {
        /// <summary>
        /// Unique IRI for the ResultDescription.
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("alignments", NullValueHandling = NullValueHandling.Ignore)]
        public List<AlignmentDType> Alignments { get; set; }
        [JsonProperty("allowedValues", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> AllowedValues { get; set; }
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }
        [JsonProperty("requiredLevel", NullValueHandling = NullValueHandling.Ignore)]
        public string RequiredLevel { get; set; }
        [JsonProperty("requiredValue", NullValueHandling = NullValueHandling.Ignore)]
        public string RequiredValue { get; set; }
        [JsonProperty("resultType", Required = Required.Always)]
        [JsonConverter(typeof(EnumExtStringConverter<ResultTypeEnum>))]
        public string ResultType { get; set; }
        [JsonProperty("rubricCriterionLevels", NullValueHandling = NullValueHandling.Ignore)]
        public List<RubricCriterionLevelDType> RubricCriterionLevels { get; set; }
        [JsonProperty("valueMin", NullValueHandling = NullValueHandling.Ignore)]
        public string ValueMin { get; set; }
        [JsonProperty("valueMax", NullValueHandling = NullValueHandling.Ignore)]
        public string ValueMax { get; set; }
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }

}
