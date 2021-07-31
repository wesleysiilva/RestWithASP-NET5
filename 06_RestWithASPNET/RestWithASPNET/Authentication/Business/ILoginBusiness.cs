using RestWithASPNET.Data.VO;

namespace RestWithASPNET.Business {
  public interface ILoginBusiness {
    TokenVO ValidateCredential(UserVO user);
    TokenVO ValidateCredential(TokenVO token);
    bool RevokeToken(string userName);
  }
}
