using BL;
using Microsoft.AspNetCore.Mvc;
using ML;
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

                        if (cliente.Nip == nip)
                        {
                            return RedirectToAction("Retirar", "Cajero", new { numeroCuenta });
                        }
                        else
                        {

                            ViewBag.Message = "Los datos son incorrectos";
                            return View("Modal");
                        }

                    }
                    else
                    {
                        ViewBag.Message = "No existen registros en la tabla ";
                        return View("Modal");
                    }
                }

                catch (Exception ex)
                {
                    result.Correct = false;
                    result.ErrorMessage = ex.Message;
                }
           
            return View(cliente);
        }

        [HttpGet]
        public IActionResult Retirar(int numeroCuenta)
        {
            ML.DetalleCuenta detalleCuenta = new ML.DetalleCuenta();
            ML.Cliente cliente = new ML.Cliente();
            ML.Result result = new ML.Result();
            using (var client = new HttpClient())
            {
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

                        cliente = (ML.Cliente)result.Object;

                    }

                }
                catch (Exception ex)
                {
                    result.Correct = false;
                    result.ErrorMessage = ex.Message;
                }
            }
            //  ML.Result result = BL.Cliente.GetByNumeroCuenta(numeroCuenta);
            
                int idCliente = cliente.IdCliente;
                using (var client = new HttpClient())
                {
                    try
                    {
                        client.BaseAddress = new Uri("http://localhost:5091/api/");
                        var responseTask = client.GetAsync("Cliente/GetByIdCliente/" + idCliente);
                        responseTask.Wait();
                        var resultAPI = responseTask.Result;
                        if (resultAPI.IsSuccessStatusCode)
                        {
                            var readTask = resultAPI.Content.ReadAsAsync<ML.Result>();
                            readTask.Wait();
                            ML.DetalleCuenta resultItemList = new ML.DetalleCuenta();
                            resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.DetalleCuenta>(readTask.Result.Object.ToString());
                            result.Object = resultItemList;

                            detalleCuenta = (ML.DetalleCuenta)result.Object;

                        }

                    }
                    catch (Exception ex)
                    {
                        result.Correct = false;
                        result.ErrorMessage = ex.Message;
                    }
                }
                //ML.Result resultSaldo = BL.Cliente.GetByIdCliente(idCliente);
                //if (resultSaldo.Correct)
                //{
                //    detalleCuenta = (ML.DetalleCuenta)resultSaldo.Object;
                //}

            return View(detalleCuenta);
        }
        [HttpPost]
        public IActionResult Retirar(ML.Cliente cliente, int retiro)
        {
            ML.Denominacion denominacion = new ML.Denominacion();
            ML.Result result = new ML.Result();
            if (cliente.Saldo >= retiro)
            {
                using (var client = new HttpClient())
                {
                    try
                    {
                        client.BaseAddress = new Uri("http://localhost:5091/api/");
                        var responseTask = client.GetAsync("Cliente/Retiro/" + retiro + "/" + cliente.Saldo);
                        responseTask.Wait();
                        var resultAPI = responseTask.Result;
                        if (resultAPI.IsSuccessStatusCode)
                        {
                            var readTask = resultAPI.Content.ReadAsAsync<ML.Result>();
                            readTask.Wait();
                            ML.Denominacion resultItemList = new ML.Denominacion();
                            resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<Denominacion>(readTask.Result.Object.ToString());
                            result.Object = resultItemList;

                            denominacion = (ML.Denominacion)result.Object;

                        }

                        responseTask = client.PostAsJsonAsync<ML.Denominacion>("Cliente/AddRetiro/" + cliente.IdCliente + "/" + retiro, denominacion);
                        responseTask.Wait();
                        resultAPI = responseTask.Result;


                        // ML.Result result1 = BL.Cliente.AddRetiro(cliente.IdCliente, retiro, denominacion);

                    }
                    catch (Exception ex)
                    {
                        result.Correct = false;
                        result.ErrorMessage = ex.Message;
                    }
                }
                return RedirectToAction("Denominacion", "Cajero", new { cliente.IdCliente });
            }
            else
            {
                ViewBag.Message = "No se cuenta con saldo suficiente";
                return View("Modal");
            }
        }
        public IActionResult Denominacion(int idCliente)
        {

            //ML.Result result = BL.Cliente.GetRetiroIdCliente(idCliente);
            ML.Result result = new ML.Result();
            ML.Retiro retiro = new ML.Retiro();
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://localhost:5091/api/");
                    var responseTask = client.GetAsync("Cliente/GetRetiro/" + idCliente);
                    responseTask.Wait();
                    var resultAPI = responseTask.Result;
                    if (resultAPI.IsSuccessStatusCode)
                    {
                        var readTask = resultAPI.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();
                        ML.Retiro resultItemList = new ML.Retiro();
                        resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Retiro>(readTask.Result.Object.ToString());
                        result.Object = resultItemList;

                        retiro = (ML.Retiro)result.Object;

                    }

                }
                catch (Exception ex)
                {
                    result.Correct = false;
                    result.ErrorMessage = ex.Message;
                }
            }

            return View(retiro);
        }
    }
}
