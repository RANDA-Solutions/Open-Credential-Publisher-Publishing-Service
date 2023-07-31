using Newtonsoft.Json;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Clr;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenCredentialPublisher.Credentials.Clrs.v1_0.Utilities
{
    public class SigningUtility
    {
        private readonly IKeyStore _keyStore;

        public SigningUtility(IKeyStore keyStore)
        {
            _keyStore = keyStore;
        }

        
        public string Sign(ClrDType clr, Uri baseUri, bool removeAfterSigned = true, OcpSigningCredentials credentials = null)
        {
            if (clr != null)
            {
                string issuerId = clr.Publisher.Id;

                //if (credentials == null)
                //{
                //    (credentials, _) = _cryptoUtility.GetSigningCredentialsByIssuerId(issuerId);
                //}

                SignProfileEndorsements(clr.Learner, baseUri, removeAfterSigned, credentials, issuerId);
                SignProfileEndorsements(clr.Publisher, baseUri, removeAfterSigned, credentials, issuerId);

                var signedAssertions = new List<string>();

                foreach (var assertion in clr.Assertions)
                {
                    SignAssertionEndorsements(assertion, baseUri, removeAfterSigned, credentials, issuerId);
                    SignAchievementEndorsements(assertion.Achievement, baseUri, removeAfterSigned, credentials, issuerId);
                    SignProfileEndorsements(assertion.Achievement?.Issuer, baseUri, removeAfterSigned, credentials, issuerId);

                    if (assertion.Verification?.Type == VerificationTypeEnum.Signed)
                    {
                        var signedAssertion = SignAssertion(assertion, baseUri, credentials);
                        clr.SignedAssertions ??= new List<string>();
                        clr.SignedAssertions.Add(signedAssertion);
                        signedAssertions.Add(assertion.Id);
                    }
                }

                if (removeAfterSigned)
                    clr.Assertions.RemoveAll(a => signedAssertions.Contains(a.Id));

                return SignClr(clr, baseUri, credentials);
            }

            return null;
        }

        /// <summary>
        /// Return the public key.
        /// </summary>
        /// <param name="keysApiUrl">The keys API URL.</param>
        /// <param name="keyId">The key id.</param>
        public CryptographicKeyDType GetCryptographicKey(Uri keysApiUrl, string keyId)
        {
            var credentials = _keyStore.GetSigningCredentialsAsync(keyId: keyId).Result;

            return GetCryptographicKey(keysApiUrl, credentials.IssuerId, credentials);
        }

        /// <summary>
        /// Return the public key.
        /// </summary>
        /// <param name="keysApiUrl">The keys API URL.</param>
        /// <param name="keyId">The key id.</param>
        public CryptographicKeyDType GetCryptographicKey(Uri keysApiUrl, string issuerId, OcpSigningCredentials credentials)
        {
            if (credentials == null)
            {
                return default;
            }

            var publicKey = _keyStore.GetPublicKeyAsync(credentials).Result;

            return new CryptographicKeyDType
            {
                Id = UriUtility.Combine(keysApiUrl, issuerId, credentials.KeyId),
                Owner = issuerId,
                PublicKeyPem = publicKey
            };
        }

        /// <summary>
        /// Get the revocation list for an issuer. The issuer is found using the issuer's key id.
        /// </summary>
        /// <param name="revocationListApiUrl"></param>
        /// <param name="keyId"></param>
        public RevocationListDType GetRevocationList(Uri revocationListApiUrl, string keyId)
        {
            var credentials = _keyStore.GetSigningCredentialsAsync(keyId: keyId).Result;

            if (credentials == null)
            {
                return default;
            }

            return new RevocationListDType
            {
                Id = UriUtility.Combine(revocationListApiUrl, keyId),
                Issuer = credentials.IssuerId,
                RevokedAssertions = new List<string>()
            };
        }

        private void SignProfileEndorsements(ProfileDType profile, Uri baseUri, bool removeAfterSigned, OcpSigningCredentials credentials, string issuerId = null)
        {
            if (profile?.Endorsements != null)
            {
                foreach (var endorsement in profile.Endorsements.Where(e => e.Verification?.Type == VerificationTypeEnum.Signed).ToList())
                {
                    if (issuerId == null || (profile.Id.Equals(issuerId, StringComparison.OrdinalIgnoreCase)))
                    {
                        var signedEndorsement = SignEndorsement(endorsement, baseUri, credentials);
                        profile.SignedEndorsements ??= new List<string>();
                        profile.SignedEndorsements.Add(signedEndorsement);
                        if (removeAfterSigned)
                            profile.Endorsements.Remove(endorsement);
                    }
                }
            }
        }

        private void SignAssertionEndorsements(AssertionDType assertion, Uri baseUri, bool removeAfterSigned, OcpSigningCredentials credentials, string issuerId = null)
        {
            if (assertion?.Endorsements != null)
            {
                foreach (var endorsement in assertion.Endorsements.Where(e => e.Verification?.Type == VerificationTypeEnum.Signed).ToList())
                {
                    if (issuerId == null || (assertion.Achievement.Issuer.Id.Equals(issuerId, StringComparison.OrdinalIgnoreCase)))
                    {
                        var signedEndorsement = SignEndorsement(endorsement, baseUri, credentials);
                        assertion.SignedEndorsements ??= new List<string>();
                        assertion.SignedEndorsements.Add(signedEndorsement);
                        if (removeAfterSigned)
                            assertion.Endorsements.Remove(endorsement);
                    }
                }
            }
        }

        private void SignAchievementEndorsements(AchievementDType achievement, Uri baseUri, bool removeAfterSigned, OcpSigningCredentials credentials, string issuerId = null)
        {
            if (achievement?.Endorsements != null)
            {
                foreach (var endorsement in achievement.Endorsements.Where(e => e.Verification?.Type == VerificationTypeEnum.Signed).ToList())
                {
                    if (issuerId == null || (achievement.Issuer.Id.Equals(issuerId, StringComparison.OrdinalIgnoreCase)))
                    {
                        var signedEndorsement = SignEndorsement(endorsement, baseUri, credentials);
                        achievement.SignedEndorsements ??= new List<string>();
                        achievement.SignedEndorsements.Add(signedEndorsement);
                        if (removeAfterSigned)
                            achievement.Endorsements.Remove(endorsement);
                    }
                }
            }
        }

        /// <summary>
        /// Return a signed (JWS) clr
        /// </summary>
        private string SignClr(ClrDType clr, Uri baseUri)
        {
            var credentials = _keyStore.GetSigningCredentialsAsync(issuerId: clr.Publisher.Id).Result;

            return SignClr(clr, baseUri, credentials);
        }

        /// <summary>
        /// Return a signed (JWS) assertion
        /// </summary>
        private string SignAssertion(AssertionDType assertion, Uri baseUri)
        {
            var credentials = _keyStore.GetSigningCredentialsAsync(issuerId: assertion.Achievement.Issuer.Id).Result;

            return SignAssertion(assertion, baseUri, credentials);
        }

        private string SignEndorsement(EndorsementDType endorsement, Uri baseUri)
        {
            var credentials = _keyStore.GetSigningCredentialsAsync(issuerId: endorsement.Issuer.Id).Result;

            return SignEndorsement(endorsement, baseUri, credentials);
        }

        private string SignClr(ClrDType clr, Uri baseUri, OcpSigningCredentials credentials)
        {
            var cryptographicKey = GetCryptographicKey(KeysEndpointUri(baseUri), clr.Publisher.Id, credentials);

            var issuer = clr.Publisher;
            cryptographicKey.Owner = issuer.Id;
            issuer.PublicKey = cryptographicKey;
            issuer.RevocationList = RevocationsListUri(baseUri, credentials.KeyId);

            clr.Verification = new VerificationDType
            {
                Type = VerificationTypeEnum.Signed
            };

            var assertionJson = JsonConvert.SerializeObject(clr);

            var token = _keyStore.SignAsync(credentials, assertionJson).Result;

            return token;
        }

        private string SignAssertion(AssertionDType assertion, Uri baseUri, OcpSigningCredentials credentials)
        {
            var cryptographicKey = GetCryptographicKey(KeysEndpointUri(baseUri), assertion.Achievement.Issuer.Id, credentials);

            var issuer = assertion.Achievement.Issuer;
            cryptographicKey.Owner = issuer.Id;
            issuer.PublicKey = cryptographicKey;
            issuer.RevocationList = RevocationsListUri(baseUri, credentials.KeyId);

            assertion.Verification = new VerificationDType
            {
                Type = VerificationTypeEnum.Signed
            };

            var assertionJson = JsonConvert.SerializeObject(assertion);

            var token = _keyStore.SignAsync(credentials, assertionJson).Result;

            return token;
        }

        private string SignEndorsement(EndorsementDType endorsement, Uri baseUri, OcpSigningCredentials credentials)
        {
            var cryptographicKey = GetCryptographicKey(KeysEndpointUri(baseUri), endorsement.Issuer.Id, credentials);

            var issuer = endorsement.Issuer;
            cryptographicKey.Owner = issuer.Id;
            issuer.PublicKey = cryptographicKey;
            issuer.RevocationList = RevocationsListUri(baseUri, credentials.KeyId);

            endorsement.Verification = new VerificationDType
            {
                Type = VerificationTypeEnum.Signed
            };

            var endorsementJson = JsonConvert.SerializeObject(endorsement);

            var token = _keyStore.SignAsync(credentials, endorsementJson).Result;

            return token;
        }

        private string RevocationsListUri(Uri baseUri, string keyId)
        {
            return UriUtility.Combine(baseUri, "revocations", keyId);
        }

        private Uri KeysEndpointUri(Uri baseUri)
        {
            return new Uri(UriUtility.Combine(baseUri, "keys"));
        }

    }

}
