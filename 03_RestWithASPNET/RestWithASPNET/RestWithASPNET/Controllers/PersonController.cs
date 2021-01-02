using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWithASPNET.Controllers {
  [ApiController]
  [Route("[controller]")]
  public class PersonController : ControllerBase {

    private readonly ILogger<PersonController> _logger;

    public PersonController(ILogger<PersonController> logger) {
      _logger = logger;
    }

    [HttpGet("sum/{firstnumber}/{secondnumber}")]
    public IActionResult Sum(string firstnumber, string secondnumber) {
      return BadRequest("Invalid input");
    }
  }
}
