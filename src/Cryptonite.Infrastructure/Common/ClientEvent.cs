using System;
using Cryptonite.Core.Enums;
using Cryptonite.Infrastructure.Abstractions.ClientEvents;
using Cryptonite.Infrastructure.Helpers;
using Newtonsoft.Json.Linq;

namespace Cryptonite.Infrastructure.Common
{
    public sealed class ClientEvent
    {
        public ClientEvent(IClientEvent innerEvent)
        {
            InnerEventJson = JObject.Parse(innerEvent.ToJson());
            Destination = innerEvent.Destination;
            CreatedAt = DateTime.UtcNow;
        }

        public JObject InnerEventJson { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public ClientEventDestinations Destination { get; private set; }
    }
}