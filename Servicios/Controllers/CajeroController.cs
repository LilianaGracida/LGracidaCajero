using Microsoft.AspNetCore.Mvc;

namespace Servicios.Controllers
{
    public class CajeroController : Controller
    {
        [HttpGet]
        [Route("api/Cliente/GetByNumeroCuenta/{numeroCuenta}")]
        public IActionResult GetByNumeroCuenta(int numeroCuenta)
        {
            var result = BL.Cliente.GetByNumeroCuenta(numeroCuenta);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("api/Cliente/GetByIdCliente/{idCliente}")]
        public IActionResult GetByIdCliente(int idCliente)
        {
            var result = BL.Cliente.GetByIdCliente(idCliente);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet]
        [Route("api/Cliente/GetDenominacion")]
        public IActionResult GetDenominacion()
        {
            var result = BL.Cliente.GetDenominacion();

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("api/Cliente/Retiro/{retiro}/{saldo}")]
        public IActionResult Retiro (int retiro, int saldo)
        {
            var result = BL.Cliente.Retiro(retiro, saldo);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost]
        [Route("api/Cliente/AddRetiro/{idCliente}/{saldo}")]
        public IActionResult AddRetiro(int idCliente, int saldo, [FromBody] ML.Denominacion denominacion)
        {
            var result = BL.Cliente.AddRetiro(idCliente, saldo,denominacion);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
