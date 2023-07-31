namespace OpenCredentialPublisher.Credentials.Clrs.v2_0
{
    public interface IVerifiableCredential
    {
        string[] Context { get; set; }
        BasicProperties[] CredentialSchema { get; set; }
        BasicProperties CredentialStatus { get; set; }
        CredentialSubject CredentialSubject { get; set; }
        string Description { get; set; }
        string ExpirationDate { get; set; }
        string Id { get; set; }
        Image Image { get; set; }
        string IssuanceDate { get; set; }
        Profile Issuer { get; set; }
        string Name { get; set; }
        Proof[] Proof { get; set; }
        BasicProperties RefreshService { get; set; }
        BasicProperties[] TermsOfUse { get; set; }
        string[] Type { get; set; }
    }
}