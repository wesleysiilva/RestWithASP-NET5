using RestWithASPNET.Data.Converter.Implementations;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;
using RestWithASPNET.Repository;
using System.Collections.Generic;

namespace RestWithASPNET.Business.Implementations {

  public class PersonBusinessImplementation : IPersonBusiness {
    private readonly IRepository<Person> _repository;
    private readonly PersonConverter _converter;

    public PersonBusinessImplementation(IRepository<Person> repository) {
      _repository = repository;
      _converter = new PersonConverter();
    }

    public List<PersonVO> FindAll() {
      return _converter.Parse(_repository.FindAll());
    }

    public PersonVO FindByID(long id) {
      return _converter.Parse(_repository.FindByID(id));
    }

    public PersonVO Create(PersonVO person) {
      var PersonEntity = _converter.Parse(person);
      PersonEntity = _repository.Create(PersonEntity);
      return _converter.Parse(PersonEntity);
    }

    public PersonVO Update(PersonVO person) {
      var PersonEntity = _converter.Parse(person);
      PersonEntity = _repository.Update(PersonEntity);
      return _converter.Parse(PersonEntity);
    }

    public void Delete(long id) {
      _repository.Delete(id);
    }
  }
}
