using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace RestWithASPNET.Hypermedia.Abstract {
  public interface IResponseEnricher {
    bool CanEnrich(ResultExecutingContext contex);
    Task Enrich(ResultExecutingContext contex);
  }
}
