using RestWithASPNET.Data.Converter.Implementation;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Hypermedia.Utils;
using RestWithASPNET.Repository;
using System.Collections.Generic;

namespace RestWithASPNET.Business.Implementations {
  public class PersonBusinessImplementation : IPersonBusiness {

    private readonly IPersonRepository _repository;
    private readonly PersonConverter _converter;

    public PersonBusinessImplementation(IPersonRepository repository) {
      _repository = repository;
      _converter = new PersonConverter();
    }

    public List<PersonVO> FindAll() {
      //return _converter.Parse(_repository.FindAll());
      string query = "";
      return _repository.FindWithPagedSearch(query);
    }

    public PagedSearchVO<PersonVO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int currentPage) {

      var sort = (!string.IsNullOrWhiteSpace(sortDirection) && !sortDirection.Equals("desc")) ? "asc" : "desc";
      var size = (pageSize < 1) ? 10 : pageSize;
      var offset = (currentPage > 0) ? (currentPage - 1) * size : 0;

      string query = @"select * FROM person p where 1 = 1 ";
      query += !string.IsNullOrWhiteSpace(name) ? $" and p.first_name like '%{name}%' " : "";
      query += $"order by p.first_name {sort} limit {size} offset {offset}";

      string countQuery = @"select count(*) FROM person p where 1 = 1 ";
      countQuery += !string.IsNullOrWhiteSpace(name) ? $" and p.first_name like '%{name}%' " : "";

      var persons = _repository.FindWithPagedSearch(query);      
      int totalResults = _repository.GetCount(countQuery);

      return new PagedSearchVO<PersonVO> { 
        CurrentPage = currentPage,
        List = _converter.Parse(persons),
        PageSize = size,
        SortDirections = sort,
        TotalResults = totalResults
      };
    }

    public PersonVO FindByID(long id) {
      return _converter.Parse(_repository.FindByID(id));
    }

    public List<PersonVO> FindByName(string firstName, string lastName) {
      return _converter.Parse(_repository.FindByName(firstName, lastName));
    }

    public PersonVO Create(PersonVO person) {
      var personEntity = _converter.Parse(person);
      personEntity = _repository.Create(personEntity);
      return _converter.Parse(personEntity);
    }

    public PersonVO Update(PersonVO person) {
      var personEntity = _converter.Parse(person);
      personEntity = _repository.Update(personEntity);
      return _converter.Parse(personEntity);
    }

    public PersonVO Disable(long id) {
      var personEntity = _repository.Disable(id);
      return _converter.Parse(personEntity);
    }

    public void Delete(long id) {
      _repository.Delete(id);
    }
  }
}
