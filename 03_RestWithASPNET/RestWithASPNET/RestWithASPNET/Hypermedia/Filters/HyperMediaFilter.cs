using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace RestWithASPNET.Hypermedia.Filters {
  public class HyperMediaFilter : ResultFilterAttribute {
    private readonly HyperMediaFilterOptions _hyperMediaFilterOptions;

    public HyperMediaFilter( HyperMediaFilterOptions options) {
      this._hyperMediaFilterOptions = options;
    }

    public override void OnResultExecuting(ResultExecutingContext context) {
      tryEnrichResult(context);
      base.OnResultExecuting(context);
    }

    private void tryEnrichResult(ResultExecutingContext context) {
      if (context.Result is OkObjectResult objectResult) {
        var enricher = _hyperMediaFilterOptions
          .ContentResponseEnricherList
          .FirstOrDefault(x => x.CanEnrich(context));

        if (enricher != null) Task.FromResult(enricher.Enrich(context));
      };
    }
  }
}
