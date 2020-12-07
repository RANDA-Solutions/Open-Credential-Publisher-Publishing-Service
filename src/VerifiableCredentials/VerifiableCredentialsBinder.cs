﻿using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenCredentialsPublisher.Credentials.VerifiableCredentials
{
    public class VerifiableCredentialsBinder : ISerializationBinder
    {
        public IList<Type> KnownTypes { get; set; }
        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            if (KnownTypes.All(t => t != serializedType))
            {
                assemblyName = null;
                typeName = null;
                return;
            }

            assemblyName = null;
            typeName = serializedType.Name;
        }

        public Type BindToType(string assemblyName, string typeName)
        {
            if (KnownTypes.All(t => t.Name != typeName))
            {
                return typeof(object);
            }

            return KnownTypes.SingleOrDefault(t => t.Name == typeName);
        }

        public static VerifiableCredentialsBinder GetBinder()
        {
            var binder = new VerifiableCredentialsBinder
            {
                KnownTypes = new List<Type>
                {
                    typeof(ClrSubject)
                }
            };
            return binder;
        }
    }
}
