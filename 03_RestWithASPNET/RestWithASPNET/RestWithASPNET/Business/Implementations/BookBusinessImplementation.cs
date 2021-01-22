using RestWithASPNET.Model;
using RestWithASPNET.Repository;
using System.Collections.Generic;

namespace RestWithASPNET.Business.Implementations {
  
  public class BookBusinessImplementation : IBookBusiness {
    private readonly IRepository<Book> _repository;

    public BookBusinessImplementation(IRepository<Book> repository) {
      _repository = repository;
    }

    public List<Book> FindAll() {
      return _repository.FindAll();
    }

    public Book FindByID(long id) {
      return _repository.FindByID(id);
    }

    public Book Create(Book person) {
      return _repository.Create(person);
    }

    public Book Update(Book person) {
      return _repository.Update(person);
    }

    public void Delete(long id) {
      _repository.Delete(id);
    }
  }
}
