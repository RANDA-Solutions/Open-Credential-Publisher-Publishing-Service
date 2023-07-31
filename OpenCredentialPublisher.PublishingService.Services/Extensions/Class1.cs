using OpenCredentialPublisher.Credentials.Clrs.v1_0.Clr;
using OpenCredentialPublisher.Credentials.Clrs.v2_0;
using OpenCredentialPublisher.PublishingService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenCredentialPublisher.PublishingService.Services.Extensions
{
    public static class AchievementDTypeExtensions
    {
        public static Achievement ToAchievement(this AchievementDType achievement)
        {
            var a = new Achievement
            {
                Id = achievement.Id,
                Type = new[] { "Achievement" },
                CreditsAvailable = achievement.CreditsAvailable,
                AchievementType = achievement.AchievementType,
                Endorsement = achievement.Endorsements.ToEndorsementCredentials(),
                Description = achievement.Description ?? achievement.Name,
                FieldOfStudy = achievement.FieldOfStudy,
                HumanCode = achievement.HumanCode,
                Image = achievement.Image.ToImage(),
                Name = achievement.Name,
                Specialization = achievement.Specialization,
                Alignment = achievement.Alignments.ToAlignments(),
                ResultDescription = achievement.ResultDescriptions.ToResultDescriptions(),
                Criteria = new Criteria
                {
                    Narrative = achievement.Name
                }
            };
            return a;
        }
    }

    public static class IdentityDTypeExtensions
    {
        public static IdentityObject ToIdentityObject(this IdentityDType identity)
        {
            var i = new IdentityObject
            {
                Type = "IdentityObject",
                Hashed = identity.Hashed,
                IdentityHash = identity.Identity,
                IdentityType = "identifier"
            };
            return i;
        } 
    }

    public static class EvidenceDTypeExtensions
    {
        public static Evidence[] ToEvidence(this EvidenceDType evidence)
        {
            var evidenceList = new List<Evidence>();
            if (evidence.Artifacts != null && evidence.Artifacts.Any())
            {
                foreach(var a in evidence.Artifacts)
                {
                    var ev = new Evidence
                    {
                        Id = a.Url,
                        Description = a.Description,
                        Name = a.Name ?? evidence.Name,
                        Audience = evidence.Audience,
                        Genre = evidence.Genre,
                        Narrative = evidence.Narrative,
                        Type = new[] { "Evidence" }
                    };
                    evidenceList.Add(ev);
                }
            }
            else
            {
                var e = new Evidence
                {
                    Id = evidence.Id,
                    Type = new[] { "Evidence" },
                    Audience = evidence.Audience,
                    Description = evidence.Description,
                    Genre = evidence.Genre,
                    Name = evidence.Name,
                    Narrative = evidence.Narrative,

                };
                evidenceList.Add(e);
            }
            return evidenceList.ToArray();
        }

        public static Evidence[] ToEvidence(this IEnumerable<EvidenceDType> evidence)
        {
            if (evidence == null)
                return null;
            return evidence.SelectMany(e => e.ToEvidence()).ToArray();
        }
    }

    public static class ResultDTypeExtensions
    {
        public static Result ToResult(this ResultDType result)
        {
            var r = new Result
            {
                AchievedLevel = result.AchievedLevel,
                Alignment = result.Alignments.ToAlignments(),
                ResultDescription = result.ResultDescription,
                Status = result.Status,
                Type = new[] { "Result" },
                Value = result.Value
            };
            return r;
        }

        public static Result[] ToResult(this IEnumerable<ResultDType> results)
        {
            if (results == null)
                return null;
            return results.Select(x => x.ToResult()).ToArray();
        }
    }

    public static class ResultDescriptionDTypeExtensions
    {
        public static ResultDescription ToResultDescription(this ResultDescriptionDType resultDescription)
        {
            var r = new ResultDescription
            {
                Id = resultDescription.Id,
                Name = resultDescription.Name,
                ResultType = resultDescription.ResultType,
                ValueMax = resultDescription.ValueMax,
                ValueMin = resultDescription.ValueMin,
                RequiredLevel = resultDescription.RequiredLevel,
                RequiredValue = resultDescription.RequiredValue,
                Alignment = resultDescription.Alignments.ToAlignments(),
                AllowedValue = resultDescription.AllowedValues?.ToArray(),
                RubricCriterionLevel = resultDescription.RubricCriterionLevels.ToRubricCriterionLevel()
            };

            return r;
        }

        public static ResultDescription[] ToResultDescriptions(this IEnumerable<ResultDescriptionDType> resultDescriptions)
        {
            if (resultDescriptions == null)
                return null;
            return resultDescriptions.Select(r => r.ToResultDescription()).ToArray();
        }
    }

    public static class AlignmentDTypeExtensions
    {
        public static Alignment ToAlignment(this AlignmentDType alignment)
        {
            var a = new Alignment
            {
                TargetCode = alignment.TargetCode,
                TargetDescription = alignment.TargetDescription,
                TargetFramework = alignment.FrameworkName,
                TargetName = alignment.TargetName,
                TargetType = alignment.TargetType,
                TargetUrl = alignment.TargetUrl,
            };
            return a;
        }

        public static Alignment[] ToAlignments(this IEnumerable<AlignmentDType> alignments)
        {
            if (alignments == null) return null;
            return alignments.Select(a => a.ToAlignment()).ToArray();
        }
    }

    public static class RubricCriterionLevelDTypeExtensions
    {
        public static RubricCriterionLevel ToRubricCriterionLevel(this RubricCriterionLevelDType rubricCriterionLevel)
        {
            var r = new RubricCriterionLevel
            {
                Id = rubricCriterionLevel.Id,
                Name = rubricCriterionLevel.Name,
                Description = rubricCriterionLevel.Description,
                Level = rubricCriterionLevel.Level,
                Points = rubricCriterionLevel.Points,
                Alignment = rubricCriterionLevel.Alignments.ToAlignments()
            };
            return r;
        }

        public static RubricCriterionLevel[] ToRubricCriterionLevel(this IEnumerable<RubricCriterionLevelDType> levels)
        {
            if (levels == null)
                return null;
            return levels.Select(l => l.ToRubricCriterionLevel()).ToArray();
        }
    }



    public static class ProfileDTypeExtensions
    {
        public static Profile ToProfile(this ProfileDType profile)
        {
            if (profile == null) return null;
            var p = new Profile
            {
                Type = new[] { "Profile" },
                ParentOrg = profile.ParentOrg.ToProfile(),
                Phone = profile.Telephone,
                DateOfBirth = profile.BirthDate?.ToString("yyyy-MM-dd") ?? null,
                AdditionalName = profile.AdditionalName,
                Description = profile.Description,
                Email = profile.Email,
                FamilyName = profile.FamilyName,
                GivenName = profile.GivenName,
                Id = profile.Id,
                Image = profile.Image?.ToImage(),
                Name = profile.Name,
                Url = profile.Url,
                Official = profile.Official,
                Endorsement = profile.Endorsements.ToEndorsementCredentials()
            };
            return p;
        }

        public static Profile ToProfile(this EndorsementProfileDType profile)
        {
            var p = new Profile
            {
                Type = new[] { "Profile" },
                Phone = profile.Telephone,
                AdditionalName = profile.AdditionalName,
                Description = profile.Description,
                Email = profile.Email,
                FamilyName = profile.FamilyName,
                GivenName = profile.GivenName,
                Id = profile.Id,
                Image = profile.Image.ToImage(),
                Name = profile.Name,
                Url = profile.Url,
                Official = profile.Official,
            };
            return p;
        }
    }

    public static class EndorsementDTypeExtensions
    {
        public static EndorsementCredential ToEndorsementCredential(this EndorsementDType endorsement)
        {
            var e = new EndorsementCredential
            {
                Id = endorsement.Id,
                Issuer = endorsement.Issuer.ToProfile(),
                IssuanceDate = endorsement.IssuedOn.ToString(Formats.DateTimeFormat),
                CredentialSubject = new EndorsementSubject
                {
                    Id = endorsement?.Claim?.Id,
                    Type = new[] { "EndorsementSubject"}, 
                    EndorsementComment = endorsement?.Claim?.EndorsementComment,
                },
                Name = "Endorsement"
            };
            return e;
        }

        public static EndorsementCredential[] ToEndorsementCredentials(this IEnumerable<EndorsementDType> endorsements)
        {
            if (endorsements == null)
            {
                return null;
            }

            return endorsements.Select(e => e.ToEndorsementCredential()).ToArray();
        }
    }

    public static class StringExtensions
    {
        public static Image ToImage(this string image)
        {
            if (String.IsNullOrWhiteSpace(image)) 
                return default;
            return new Image { Id = image, Type = "Image" };
        } 
    }
}
