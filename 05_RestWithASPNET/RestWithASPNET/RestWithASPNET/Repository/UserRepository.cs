using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;
using RestWithASPNET.Model.Context;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RestWithASPNET.Repository {
  public class UserRepository : IUserRepository {
    private readonly SqlServerContext _context;

    public UserRepository(SqlServerContext context) {
      _context = context;
    }

    public User ValidateCredentials(string userName) {      
      return _context.User.SingleOrDefault(u => (u.UserName == userName));
    }

    public User ValidateCredentials(UserVO user) {
      var pass = ComputeHash(user.Password, new SHA256CryptoServiceProvider());
      return _context.User.FirstOrDefault(u => (u.UserName == user.UserName) && (u.Password == pass));
    }

    public bool RevokeToken(string userName) {
      var user = _context.User.SingleOrDefault(u => (u.UserName == userName));
      if (user == null) return false;
      user.RefreshToken = null;
      _context.SaveChanges();
      return true;
    }

    public User RefreshUserInfo(User user) {
      if (!_context.User.Any(p => p.Id.Equals(user.Id))) return null;

      var result = _context.User.SingleOrDefault(p => p.Id.Equals(user.Id));
      if (result != null) {
        try {
          _context.Entry(result).CurrentValues.SetValues(user);
          _context.SaveChanges();
          return result;
        } catch (Exception e) {
          throw;
        }
      }
      return result;
    }

    private string ComputeHash(string input, SHA256CryptoServiceProvider algorithm) {
      Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
      Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);
      return BitConverter.ToString(hashedBytes);
    }
  }
}
