using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Credentials.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class JsonTypeAttribute: Attribute
    {
        public string Name { get; set; }
        public JsonTypeAttribute(string name)
        {
            Name = name;
        }
    }
}
