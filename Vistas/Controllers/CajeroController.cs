using BL;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity.Core.Metadata.Edm;
using System.Drawing.Drawing2D;
using System.Net;

namespace Vistas.Controllers
{
    public class CajeroController : Controller
    {
        public IActionResult Inicio()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ValidarCuenta(int numeroCuenta,int nip)
        {
            ML.Cliente cliente = new ML.Cliente();
            ML.Result result = new ML.Result();
            using (var client = new HttpClient())
                try
                {
                    client.BaseAddress = new Uri("http://localhost:5091/api/");
                    var responseTask = client.GetAsync("Cliente/GetByNumeroCuenta/" + numeroCuenta);
                    responseTask.Wait();
                    var resultAPI = responseTask.Result;
                    if (resultAPI.IsSuccessStatusCode)
                    {
                        var readTask = resultAPI.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();
                        ML.Cliente resultItemList = new ML.Cliente();
                        resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cliente>(readTask.Result.Object.ToString());
                        result.Object = resultItemList;

                        cliente = ((ML.Cliente)result.Object);
                     return RedirectToAction("Retirar", "Cajero", new { numeroCuenta });

                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No existen registros en la tabla ";
                        return View("Modal");
                    }
                }

                catch (Exception ex)
                {
                    result.Correct = false;
                    result.ErrorMessage = ex.Message;
                }
            //ML.Result result = BL.Cliente.GetByNumeroCuenta(numeroCuenta);
            //ML.Cliente cliente = new ML.Cliente();
            //if (result.Correct)
            //{
            //    cliente =(ML.Cliente)result.Object;
            //    if(cliente.Nip == nip)
            //    {
            //        return RedirectToAction("Retirar", "Cajero", new {numeroCuenta});
            //    }
            //}
            //else
            //{

            //}
            return View(cliente);
        }

        [HttpGet]
        public IActionResult Retirar(int numeroCuenta)
        {
            ML.DetalleCuenta detalleCuenta = new ML.DetalleCuenta();
            ML.Result result = BL.Cliente.GetByNumeroCuenta(numeroCuenta);
            ML.Cliente cliente = new ML.Cliente();
            if (result.Correct)
            {
                cliente = (ML.Cliente)result.Object;
                int idCliente = cliente.IdCliente;
                //using(var client = new HttpClient())
                //{
                //    try
                //    {
                //        client.BaseAddress = new Uri("http://localhost:5091/api/");
                //        var responseTask = client.GetAsync("Cliente/GetByIdCliente/" + idCliente);
                //        responseTask.Wait();
                //        var resultAPI = responseTask.Result;
                //        if (resultAPI.IsSuccessStatusCode)
                //        {
                //            var readTask = resultAPI.Content.ReadAsAsync<ML.Result>();
                //            readTask.Wait();
                //            ML.DetalleCuenta resultItemList = new ML.DetalleCuenta();
                //            resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.DetalleCuenta>(readTask.Result.Object.ToString());
                //            result.Object = resultItemList;

                //            detalleCuenta = (ML.DetalleCuenta)result.Object;

                //        }

                //    }
                //    catch (Exception ex)
                //    {
                //        result.Correct = false;
                //        result.ErrorMessage = ex.Message;
                //    }
                //}
                ML.Result resultSaldo = BL.Cliente.GetByIdCliente(idCliente);
                if (resultSaldo.Correct)
                {
                    detalleCuenta = (ML.DetalleCuenta)resultSaldo.Object;
                }
            }

                return View(detalleCuenta);
        }
        [HttpPost]
        public IActionResult Retirar(ML.Cliente cliente, int retiro)
        {
            ML.Denominacion denominacion = new ML.Denominacion();
            ML.Retiro retiro1 = new ML.Retiro();
            ML.Result result = BL.Cliente.Retiro(retiro, cliente.Saldo);
            if (result.Correct)
            {
                denominacion = (ML.Denominacion)result.Object;

                ML.Result result1 = BL.Cliente.AddRetiro(cliente.IdCliente, retiro,denominacion);

            }
            
            return RedirectToAction("Denominacion", "Cajero", cliente.IdCliente);
         }
        public IActionResult Denominacion(int idCliente)
        {
            ML.Result result = BL.Cliente.GetRetiroIdCliente(idCliente);
            ML.Retiro retiro = new ML.Retiro();
            if (result.Correct)
            {
                retiro = (ML.Retiro)result.Object;
            }

            return View(retiro);
        }
    }
}
