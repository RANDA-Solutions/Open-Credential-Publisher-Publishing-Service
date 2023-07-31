using OpenCredentialPublisher.PublishingService.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Services.Abstractions
{
    public interface ITransformationService<TSource, TTarget> 
        where TSource : class, new()
        where TTarget : class, new()
    {
        Task<TTarget> Transform(string appBaseUri, PublishRequest publishRequest, TSource source);
    }
}
