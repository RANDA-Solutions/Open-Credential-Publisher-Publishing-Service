using NUnit.Framework;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Credentials.Tests
{
    public class UriTests
    {
        private readonly Uri baseUri = new Uri("https://www.example.org/");

        [Test]
        public void NoPartsTest()
        {
            var urlString = UriUtility.Combine(baseUri);
            Assert.IsTrue(String.Equals(urlString, baseUri.ToString()));
        }

        [Test]
        public void RelativeUrlTest()
        {
            var uri = new Uri("/here/it/is", UriKind.Relative);
            var urlString = UriUtility.Combine(uri);
            Assert.IsTrue(String.Equals(urlString, uri.ToString()));
        }

        [Test]
        public void LocalhostTest()
        {
            var uri = new Uri("http://localhost/here/it/is");
            var urlString = UriUtility.Combine(uri);
            Assert.IsTrue(String.Equals(urlString, uri.ToString()));
        }

        [Test]
        public void QueryStringTest()
        {
            const string query = "?random=20&six=40";
            const string part = "customer";
            var urlString = UriUtility.Combine(baseUri, part, query);

            var validUri = new Uri(urlString);
            Assert.IsNotNull(validUri);
        }

        [Test]
        public void MultiplePartsTest()
        {
            string[] parts = new[] { "one", "two", "three", "/four", "five/", "six" };
            var urlString = UriUtility.Combine(baseUri, parts);
            Assert.AreEqual(urlString, $"{baseUri}one/two/three/four/five/six");
        }

        [Test]
        public void NoDoubleSlash()
        {
            string[] parts = new[] { "one", "two", "three", "/four", "five/", "six" };
            var urlString = UriUtility.Combine(baseUri, parts);
            Assert.IsFalse(urlString.Split("//").Length > 2);
        }
    }
}
