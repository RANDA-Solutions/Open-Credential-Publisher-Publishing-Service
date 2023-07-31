using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenCredentialPublisher.Credentials.Clrs.v1_0.Utilities
{
    public static class UriUtility
    {
        public static String Combine(Uri baseUri, params string[] parts)
        {
            const char separator = '/';
            var hasParts = parts != null && parts.Length > 0;

            if (baseUri.OriginalString.StartsWith(separator))
            {
                var uriString = baseUri.ToString().TrimEnd(separator);
                if (hasParts)
                {
                    var partsList = new List<String>() { { uriString } };
                    partsList.AddRange(parts.Select(p => p.Trim(separator)));
                    uriString = String.Join(separator, partsList);
                }
                return uriString;
            }
            var uriBuilder = new UriBuilder(baseUri);
            if (hasParts)
            {
                var partsList = new List<String>();
                if (!String.IsNullOrEmpty(uriBuilder.Path))
                    partsList.Add(uriBuilder.Path.TrimEnd(separator));
                partsList.AddRange(parts.Select(p => p.Trim(separator)));
                uriBuilder.Path = String.Join(separator, partsList);
            }
            return uriBuilder.Uri.AbsoluteUri;
        }
    }
}
