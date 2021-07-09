using Microsoft.IdentityModel.JsonWebTokens;
using RestWithASPNET.Configurations;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;
using RestWithASPNET.Repository;
using RestWithASPNET.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace RestWithASPNET.Business.Implementations {
  public class LoginBusinessImplementation : ILoginBusiness {
    private const string DATE_FORMAT = "yyy-MM-dd HH:mm:ss";
    private TokenConfiguration _configuration;
    private IUserRepository _repository;
    private readonly ITokenService _tokenService;

    public LoginBusinessImplementation(TokenConfiguration configuration, IUserRepository repository, ITokenService tokenService) {
      _configuration = configuration;
      _repository = repository;
      _tokenService = tokenService;
    }

    public TokenVO ValidateCredential(UserVO userCredentials) {
      var user = _repository.ValidateCredentials(userCredentials);
      if (user == null) return null;
      var claims = new List<Claim> {
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
      };

      var accessToken = _tokenService.GenerateAccessToken(claims);
      var refreshToken = _tokenService.GenerateRefreshToken();

      user.RefreshToken = refreshToken;
      user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_configuration.DaysToExpiry);

      _repository.RefreshUserInfo(user);

      DateTime createDate = DateTime.Now;
      DateTime expirationDate = createDate.AddMinutes(_configuration.Minutes);

      return new TokenVO(
        true,
        createDate.ToString(DATE_FORMAT),
        expirationDate.ToString(DATE_FORMAT),
        accessToken,
        refreshToken
      );
    }

    public TokenVO ValidateCredential(TokenVO token) {
      var accessToken = token.AccessToken;
      var refreshToken = token.RefreshToken;
      var principal = _tokenService.GetPrincipalFromExpiryToken(accessToken);
      var userName = principal.Identity.Name;
      var user = _repository.ValidateCredentials(userName);

      if (
        user == null || 
        user.RefreshToken != refreshToken || 
        user.RefreshTokenExpiryTime <= DateTime.Now
      ) return null;

      accessToken = _tokenService.GenerateAccessToken(principal.Claims);
      refreshToken = _tokenService.GenerateRefreshToken();

      user.RefreshToken = refreshToken;
      _repository.RefreshUserInfo(user);

      DateTime createDate = DateTime.Now;
      DateTime expirationDate = createDate.AddMinutes(_configuration.Minutes);

      return new TokenVO(
        true,
        createDate.ToString(DATE_FORMAT),
        expirationDate.ToString(DATE_FORMAT),
        accessToken,
        refreshToken
      );
    }

    public bool RevokeToken(string userName) {
      return _repository.RevokeToken(userName);
    }
  }
}
