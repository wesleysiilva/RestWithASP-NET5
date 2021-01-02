using RestWithASPNET.Model;
using System.Collections.Generic;

namespace RestWithASPNET.Services {
  public interface IPersonService {
    Person Create(Person person);
    Person Update(Person person);
    List<Person> FindAll();
    Person FindByID(long id);
    void Delete(long id);
  }
}
