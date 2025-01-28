using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Infrastructure.Config
{
    public class JWTBearerTokenSettings
    {
        public required string SecretKey { get; set; }
        public required string Audience { get; set; }
        public required string Issuer { get; set; }
        public int ExpiryTimeInSeconds { get; set; }
    }
}