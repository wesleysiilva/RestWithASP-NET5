using Microsoft.EntityFrameworkCore.Internal;
using RestWithASPNET.Model;
using RestWithASPNET.Model.Context;
using RestWithASPNET.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestWithASPNET.Business.Implementations {
  
  public class BookBusinessImplementation : IBookBusiness {
    private readonly IBookRepository _repository;

    public BookBusinessImplementation(IBookRepository repository) {
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
