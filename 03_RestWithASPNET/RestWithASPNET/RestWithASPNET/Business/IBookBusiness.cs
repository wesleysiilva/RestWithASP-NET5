using RestWithASPNET.Model;
using System.Collections.Generic;

namespace RestWithASPNET.Business {
  public interface IBookBusiness {
    Book Create(Book book);
    Book Update(Book book);
    List<Book> FindAll();
    Book FindByID(long id);
    void Delete(long id);
  }
}
