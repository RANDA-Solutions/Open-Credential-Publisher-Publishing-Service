using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenCredentialsPublisher.Credentials.Clrs.Utilities
{
    public static class UriUtility
    {
        public static String Combine(Uri baseUri, params string[] parts)
        {
            const char separator = '/';
            var hasParts = parts != null && parts.Length > 0;
            var baseUriString = baseUri.ToString();
            if (baseUriString.EndsWith(separator) || hasParts)
            {
                // Trim all trailing '/' to ensure no excess 
                baseUriString = baseUriString.TrimEnd(separator) + separator;
            }
            var builder = new StringBuilder(baseUriString);
            if (hasParts)
                builder = builder.AppendJoin(separator, parts.Select(p => p.Trim(separator)));
            return builder.ToString();
        }
    }
}
