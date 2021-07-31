using RestWithASPNET.Data.VO;
using RestWithASPNET.Hypermedia.Utils;
using System.Collections.Generic;

namespace RestWithASPNET.Business {
  public interface IPersonBusiness {
    PersonVO Create(PersonVO person);
    PersonVO FindByID(long id);
    List<PersonVO> FindByName(string firstName, string lastName);
    List<PersonVO> FindAll();
    PersonVO Update(PersonVO person);
    void Delete(long id);
    PersonVO Disable(long id);
    PagedSearchVO<PersonVO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int currentPage);
  }
}
