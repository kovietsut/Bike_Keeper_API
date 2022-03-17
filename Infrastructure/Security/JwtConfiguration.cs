using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security
{
    public class JwtConfiguration
    {
        public string Issuer { get; set; }
        public TimeSpan ValidFor { get; set; } = TimeSpan.FromDays(30 * 3);
        public DateTime Expiration => IssuedAt.Add(ValidFor);
        public string Subject { get; set; }
        public string Audience { get; set; }
        public DateTime NotBefore => DateTime.UtcNow.AddHours(-1);
        public DateTime IssuedAt => DateTime.UtcNow.AddHours(-1);
        public Func<Task<string>> JtiGenerator =>
          () => Task.FromResult(Guid.NewGuid().ToString());
        public SigningCredentials SigningCredentials { get; set; }
    }
}
