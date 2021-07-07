﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestWithASPNET.Business;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Hypermedia.Filters;

namespace RestWithASPNET.Controllers {
  [ApiVersion("1")] //Versão da API
  [ApiController]
  [Route("api/[controller]/v{version:apiVersion}")]
  public class BookController : ControllerBase {

    //Variáveis de escopo privado, começar a nomenclatura com _
    private readonly ILogger<BookController> _logger;
    private IBookBusiness _bookBusiness;

    public BookController(ILogger<BookController> logger, IBookBusiness bookBusiness) {
      _logger = logger;
      _bookBusiness = bookBusiness;
    }

    //Maps GET requests to https://localhost:{port}/api/book/
    //Get with parameters for FindAll -> Find All
    [HttpGet]
    [TypeFilter(typeof(HyperMediaFilter))]
    public IActionResult Get() {
      return Ok(_bookBusiness.FindAll());
    }
    
    //Maps GET requests to https://localhost:{port}/api/book/{id}
    //receiveing an ID as in the Request Path
    //Get with parameters for FindById -> Search by ID
    [HttpGet("{id}")]
    [TypeFilter(typeof(HyperMediaFilter))]
    public IActionResult Get(long id) {
      var book = _bookBusiness.FindByID(id);
      if (book == null) return NotFound();
      return Ok(book);
    }

    //Maps POST requests to https://localhost:{port}/api/book/
    //[FromBody] consumes the JSON object sent in the request body
    [HttpPost]
    [TypeFilter(typeof(HyperMediaFilter))]
    public IActionResult Post([FromBody] BookVO book ) {      
      if (book == null) return BadRequest();
      return Ok(_bookBusiness.Create(book));
    }

    //Maps PUT requests to https://localhost:{port}/api/book/
    //[FromBody] consumes the JSON object sent in the request body
    [HttpPut]
    [TypeFilter(typeof(HyperMediaFilter))]
    public IActionResult Put([FromBody] BookVO book) {
      if (book == null) return BadRequest();
      return Ok(_bookBusiness.Update(book));
    }

    //Maps DELETE requests to https://localhost:{port}/api/book/{id}
    //receiveing an ID as in the Request Path
    [HttpDelete("{id}")]
    public IActionResult Delete(long id) {
      _bookBusiness.Delete(id);      
      return NoContent();
    }
  }
}
