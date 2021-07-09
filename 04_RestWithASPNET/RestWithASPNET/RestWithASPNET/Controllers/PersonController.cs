using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestWithASPNET.Business;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Hypermedia.Filters;
using System.Collections.Generic;

namespace RestWithASPNET.Controllers {
  [ApiVersion("1")] //Versão da API
  [ApiController]
  [Authorize("Bearer")]
  [Route("api/[controller]/v{version:apiVersion}")]
  public class PersonController : ControllerBase {

    //Variáveis de escopo privado, começar a nomenclatura com _
    private readonly ILogger<PersonController> _logger;
    private IPersonBusiness _personBusiness;

    public PersonController(ILogger<PersonController> logger, IPersonBusiness personBusiness) {
      _logger = logger;
      _personBusiness = personBusiness;
    }

    //Maps GET requests to https://localhost:{port}/api/person/
    //Get with parameters for FindAll -> Find All
    [HttpGet]
    [ProducesResponseType((200), Type = typeof(List<PersonVO>))]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [TypeFilter(typeof(HyperMediaFilter))]
    public IActionResult Get() {
      return Ok(_personBusiness.FindAll());
    }
    
    //Maps GET requests to https://localhost:{port}/api/person/{id}
    //receiveing an ID as in the Request Path
    //Get with parameters for FindById -> Search by ID
    [HttpGet("{id}")]
    [ProducesResponseType((200), Type = typeof(PersonVO))]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [TypeFilter(typeof(HyperMediaFilter))]
    public IActionResult Get(long id) {
      var person = _personBusiness.FindByID(id);
      if (person == null) return NotFound();
      return Ok(person);
    }

    //Maps POST requests to https://localhost:{port}/api/person/
    //[FromBody] consumes the JSON object sent in the request body
    [HttpPost]
    [ProducesResponseType((200), Type = typeof(PersonVO))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [TypeFilter(typeof(HyperMediaFilter))]
    public IActionResult Post([FromBody] PersonVO person ) {      
      if (person == null) return BadRequest();
      return Ok(_personBusiness.Create(person));
    }

    //Maps PUT requests to https://localhost:{port}/api/person/
    //[FromBody] consumes the JSON object sent in the request body
    [HttpPut]
    [ProducesResponseType((200), Type = typeof(PersonVO))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [TypeFilter(typeof(HyperMediaFilter))]
    public IActionResult Put([FromBody] PersonVO person) {
      if (person == null) return BadRequest();
      return Ok(_personBusiness.Update(person));
    }

    //Maps PATCH requests to https://localhost:{port}/api/person/
    [HttpPatch("{id}")]
    [ProducesResponseType((200), Type = typeof(List<PersonVO>))]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [TypeFilter(typeof(HyperMediaFilter))]
    public IActionResult Patch(long id) {
      var person = _personBusiness.Disable(id);
      return Ok(person);
    }

    //Maps DELETE requests to https://localhost:{port}/api/person/{id}
    //receiveing an ID as in the Request Path
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public IActionResult Delete(long id) {
      _personBusiness.Delete(id);      
      return NoContent();
    }
  }
}
