using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWithASPNET.Controllers {
  [ApiController]
  [Route("[controller]")]
  public class CalculatorController : ControllerBase {

    private readonly ILogger<CalculatorController> _logger;

    public CalculatorController(ILogger<CalculatorController> logger) {
      _logger = logger;
    }

    [HttpGet("sum/{firstnumber}/{secondnumber}")]
    public IActionResult Sum(string firstnumber, string secondnumber) {
      if (IsNumeric(firstnumber) && IsNumeric(secondnumber)) {
        var sum = ConvertToDecimal(firstnumber) + ConvertToDecimal(secondnumber);
        return Ok(sum.ToString());
      }
      return BadRequest("Invalid input");
    }

    [HttpGet("subtraction/{firstnumber}/{secondnumber}")]
    public IActionResult Subtraction(string firstnumber, string secondnumber) {
      if (IsNumeric(firstnumber) && IsNumeric(secondnumber)) {
        var sum = ConvertToDecimal(firstnumber) - ConvertToDecimal(secondnumber);
        return Ok(sum.ToString());
      }
      return BadRequest("Invalid input");
    }

    [HttpGet("multiplication/{firstnumber}/{secondnumber}")]
    public IActionResult Multiplication(string firstnumber, string secondnumber) {
      if (IsNumeric(firstnumber) && IsNumeric(secondnumber)) {
        var sum = ConvertToDecimal(firstnumber) * ConvertToDecimal(secondnumber);
        return Ok(sum.ToString());
      }
      return BadRequest("Invalid input");
    }

    [HttpGet("division/{firstnumber}/{secondnumber}")]
    public IActionResult Division(string firstnumber, string secondnumber) {
      if (IsNumeric(firstnumber) && IsNumeric(secondnumber)) {
        var sum = ConvertToDecimal(firstnumber) / ConvertToDecimal(secondnumber);
        return Ok(sum.ToString());
      }
      return BadRequest("Invalid input");
    }

    [HttpGet("mean/{firstnumber}/{secondnumber}")]
    public IActionResult mean(string firstnumber, string secondnumber) {
      if (IsNumeric(firstnumber) && IsNumeric(secondnumber)) {
        var sum = (ConvertToDecimal(firstnumber) + ConvertToDecimal(secondnumber)) / 2;
        return Ok(sum.ToString());
      }
      return BadRequest("Invalid input");
    }

    [HttpGet("squareRoot/{firstnumber}")]
    public IActionResult squareRoot(string firstnumber) {
      if (IsNumeric(firstnumber)) {
        var squareRoot = Math.Sqrt((double)ConvertToDecimal(firstnumber));
        return Ok(squareRoot.ToString());
      }
      return BadRequest("Invalid input");
    }

    private bool IsNumeric(string strNumber) {
      double number;
      bool isNumber = double.TryParse(
                                      strNumber,
                                      System.Globalization.NumberStyles.Any,
                                      System.Globalization.NumberFormatInfo.InvariantInfo,
                                      out number
                                    );
      return isNumber;
    }

    private decimal ConvertToDecimal(string strNumber) {
      decimal decimalValue;
      if (decimal.TryParse(strNumber, out decimalValue)) {
        return decimalValue;
      }
      return 0;
    }
  }
}
