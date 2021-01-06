using RestWithASPNET.Model;
using System.Collections.Generic;

namespace RestWithASPNET.Repository {
  public interface IPersonRepository {
    Person Create(Person person);
    Person Update(Person person);
    List<Person> FindAll();
    Person FindByID(long id);
    void Delete(long id);
    bool Exists(long id);
  }
}
