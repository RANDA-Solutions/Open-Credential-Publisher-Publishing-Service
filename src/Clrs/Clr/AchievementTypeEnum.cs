using System.Runtime.Serialization;

namespace OpenCredentialsPublisher.Credentials.Clrs.Clr
{

    public enum AchievementTypeEnum {
        [EnumMember(Value = "Achievement")]
        Achievement,
        [EnumMember(Value = "Assessment Result")]
        AssessmentResult,
        [EnumMember(Value = "Award")]
        Award,
        [EnumMember(Value = "Badge")]
        Badge,
        [EnumMember(Value = "Certificate")]
        Certificate,
        [EnumMember(Value = "Certification")]
        Certification,
        [EnumMember(Value = "Course")]
        Course,
        [EnumMember(Value = "Community Service")]
        CommunityService,
        [EnumMember(Value = "Competency")]
        Competency,
        [EnumMember(Value = "Co-Curricular")]
        CoCurricular,
        [EnumMember(Value = "Degree")]
        Degree,
        [EnumMember(Value = "Diploma")]
        Diploma,
        [EnumMember(Value = "Fieldwork")]
        Fieldwork,
        [EnumMember(Value = "License")]
        License,
        [EnumMember(Value = "Membership")]
        Membership
    }

}
