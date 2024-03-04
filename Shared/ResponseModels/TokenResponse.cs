using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ResponseModels
{
    public class TokenResponse
    {
        public bool? isTokenExpired {  get; set; }
        public string? Token { get; set; }

        public DateTime? expiresAt { get; set; }
    }
}
