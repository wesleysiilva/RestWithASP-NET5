using RestWithASPNET.Data.Converter.Implementations;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;
using RestWithASPNET.Repository;
using System.Collections.Generic;

namespace RestWithASPNET.Business.Implementations {
  
  public class BookBusinessImplementation : IBookBusiness {
    private readonly IRepository<Book> _repository;
    private readonly BookConverter _converter;

    public BookBusinessImplementation(IRepository<Book> repository) {
      _repository = repository;
      _converter = new BookConverter();
    }

    public List<BookVO> FindAll() {
      return _converter.Parse(_repository.FindAll());
    }

    public BookVO FindByID(long id) {
      return _converter.Parse(_repository.FindByID(id));
    }

    public BookVO Create(BookVO book) {
      var BookEntity = _converter.Parse(book);
      BookEntity = _repository.Create(BookEntity);
      return _converter.Parse(BookEntity);
    }

    public BookVO Update(BookVO book) {
      var BookEntity = _converter.Parse(book);
      BookEntity = _repository.Update(BookEntity);
      return _converter.Parse(BookEntity);
    }

    public void Delete(long id) {
      _repository.Delete(id);
    }
  }
}
