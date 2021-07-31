using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestWithASPNET.Business;
using RestWithASPNET.Data.VO;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RestWithASPNET.Controllers {

  [ApiVersion("1")] //Versão da API
  [ApiController]
  [Authorize("Bearer")]
  [Route("api/[controller]/v{version:apiVersion}")]
  public class FileController : Controller {
    //Variáveis de escopo privado, começar a nomenclatura com _
    private readonly ILogger<FileController> _logger;
    private IFileBusiness _fileBusiness;

    public FileController(ILogger<FileController> logger, IFileBusiness bookBusiness) {
      _logger = logger;
      _fileBusiness = bookBusiness;
    }
    
    [HttpPost("uploadFile")]
    [ProducesResponseType((200), Type = typeof(FileDetailVO))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Produces("application/json")]
    public async Task<IActionResult> UploadOneFile([FromForm] IFormFile file) {
      FileDetailVO detail = await _fileBusiness.SaveFileToDisk(file);
      return new OkObjectResult(detail);
    }

    [HttpPost("uploadMultipleFile")]
    [ProducesResponseType((200), Type = typeof(List<FileDetailVO>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Produces("application/json")]
    public async Task<IActionResult> UploadManyFile([FromForm] List<IFormFile> files) {
      List<FileDetailVO> details = await _fileBusiness.SaveFilesToDisk(files);
      return new OkObjectResult(details);
    }

    [HttpGet("downloadFile/{fileName}")]
    [ProducesResponseType((200), Type = typeof(byte[]))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(401)]
    [Produces("application/octet-stream")]
    public async Task<IActionResult> GetFileAsync(string fileName) {
      byte [] buffer = _fileBusiness.GetFile(fileName);
      if (buffer != null) {
        HttpContext.Response.ContentType = $"application/{Path.GetExtension(fileName).Replace(".", "")}";
        HttpContext.Response.Headers.Add("content-length", buffer.Length.ToString());
        await HttpContext.Response.Body.WriteAsync(buffer, 0, buffer.Length);
      }
      return new ContentResult();
    }
  }
}
