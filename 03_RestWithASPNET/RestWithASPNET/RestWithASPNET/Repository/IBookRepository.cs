using RestWithASPNET.Model;
using System.Collections.Generic;

namespace RestWithASPNET.Repository {
  public interface IBookRepository {
    Book Create(Book book);
    Book Update(Book book);
    List<Book> FindAll();
    Book FindByID(long id);
    void Delete(long id);
    bool Exists(long id);
  }
}
