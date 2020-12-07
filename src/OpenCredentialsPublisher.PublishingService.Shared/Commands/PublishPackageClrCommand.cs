﻿using MediatR;

namespace OpenCredentialsPublisher.PublishingService.Shared
{
    public class PublishPackageClrCommand : ICommand, INotification
    {
        public string RequestId { get; }

        public PublishPackageClrCommand(string requestId)
        {
            RequestId = requestId;
        }
    }

}
