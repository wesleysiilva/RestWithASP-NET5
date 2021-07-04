using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestWithASPNET.Model;
using RestWithASPNET.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWithASPNET.Controllers {
  [ApiController]
  [Route("api/[controller]")]
  public class PersonController : ControllerBase {

    //Variáveis de escopo privado, começar a nomenclatura com _
    private readonly ILogger<PersonController> _logger;
    private IPersonService _personService;

    public PersonController(ILogger<PersonController> logger, IPersonService personService) {
      _logger = logger;
      _personService = personService;
    }

    //Maps GET requests to https://localhost:{port}/api/person/
    //Get with parameters for FindAll -> Find All
    [HttpGet]
    public IActionResult Get() {
      return Ok(_personService.FindAll());
    }
    
    //Maps GET requests to https://localhost:{port}/api/person/{id}
    //receiveing an ID as in the Request Path
    //Get with parameters for FindById -> Search by ID
    [HttpGet("{id}")]
    public IActionResult Get(long id) {
      var person = _personService.FindByID(id);
      if (person == null) return NotFound();
      return Ok(person);
    }

    //Maps POST requests to https://localhost:{port}/api/person/
    //[FromBody] consumes the JSON object sent in the request body
    [HttpPost]
    public IActionResult Post([FromBody] Person person ) {      
      if (person == null) return BadRequest();
      return Ok(_personService.Create(person));
    }

    //Maps PUT requests to https://localhost:{port}/api/person/
    //[FromBody] consumes the JSON object sent in the request body
    [HttpPut]
    public IActionResult Put([FromBody] Person person) {
      if (person == null) return BadRequest();
      return Ok(_personService.Update(person));
    }

    //Maps DELETE requests to https://localhost:{port}/api/person/{id}
    //receiveing an ID as in the Request Path
    [HttpDelete("{id}")]
    public IActionResult Delete(long id) {
      _personService.Delete(id);      
      return NoContent();
    }
  }
}
