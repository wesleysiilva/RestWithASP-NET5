using RestWithASPNET.Data.Converter.Contract;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;
using System.Collections.Generic;
using System.Linq;

namespace RestWithASPNET.Data.Converter.Implementations {
  public class BookConverter : IParser<BookVO, Book>, IParser<Book, BookVO> {
    public Book Parse(BookVO origin) {
      if (origin == null) return null;
      return new Book {
        Id = origin.Id,
        Author = origin.Author,
        Launch_date = origin.Launch_date,
        Price = origin.Price,
        Title = origin.Title
      };
    }

    public BookVO Parse(Book origin) {
      if (origin == null) return null;
      return new BookVO {
        Id = origin.Id,
        Author = origin.Author,
        Launch_date = origin.Launch_date,
        Price = origin.Price,
        Title = origin.Title
      };
    }

    public List<Book> Parse(List<BookVO> origin) {
      if (origin == null) return null;
      return origin.Select(item => Parse(item)).ToList();
    }

    public List<BookVO> Parse(List<Book> origin) {
      if (origin == null) return null;
      return origin.Select(item => Parse(item)).ToList();
    }
  }
}