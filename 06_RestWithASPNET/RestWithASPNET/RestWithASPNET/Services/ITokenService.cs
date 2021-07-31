using System.Collections.Generic;
using System.Security.Claims;

namespace RestWithASPNET.Services {
  public interface ITokenService {
    string GenerateAccessToken(IEnumerable<Claim> claimn);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiryToken(string token);
  }
}
