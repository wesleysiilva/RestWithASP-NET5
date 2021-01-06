using RestWithASPNET.Model;
using System.Collections.Generic;

namespace RestWithASPNET.Business {
  public interface IPersonBusiness {
    Person Create(Person person);
    Person Update(Person person);
    List<Person> FindAll();
    Person FindByID(long id);
    void Delete(long id);
  }
}
