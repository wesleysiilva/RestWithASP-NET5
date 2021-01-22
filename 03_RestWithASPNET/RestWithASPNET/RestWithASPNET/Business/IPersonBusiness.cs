using RestWithASPNET.Data.VO;
using System.Collections.Generic;

namespace RestWithASPNET.Business {
  public interface IPersonBusiness {
    PersonVO Create(PersonVO person);
    PersonVO Update(PersonVO person);
    List<PersonVO> FindAll();
    PersonVO FindByID(long id);
    void Delete(long id);
  }
}
