using ML;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;

namespace BL
{
    public class Cliente
    {
        public static ML.Result GetByNumeroCuenta(int numeroCuenta)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (SqlConnection context = new SqlConnection(DL.Conexion.GetConnectionString()))
                {
                    string query = "ClienteGetByCuenta";
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = context;
                        cmd.CommandText = query;
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter[] collection = new SqlParameter[1];
                        collection[0] = new SqlParameter("NumeroCuenta", SqlDbType.Int);
                        collection[0].Value = numeroCuenta;
                        cmd.Parameters.AddRange(collection);
                        cmd.Connection.Open();

                        DataTable tableCliente = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                        adapter.Fill(tableCliente);

                        if (tableCliente.Rows.Count > 0)
                        {
                            DataRow row = tableCliente.Rows[0];
                            ML.Cliente cliente = new ML.Cliente();
                            cliente.IdCliente = int.Parse(row[0].ToString());
                            cliente.Nombre = row[1].ToString();
                            cliente.ApellidoPaterno = row[2].ToString();
                            cliente.ApellidoMaterno = row[3].ToString();
                            cliente.NumeroCuenta = int.Parse(row[4].ToString());
                            cliente.Nip = int.Parse(row[5].ToString());

                            result.Object = cliente;
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = " No existen registros en la tabla";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
        public static ML.Result GetByIdCliente(int idCliente)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (SqlConnection context = new SqlConnection(DL.Conexion.GetConnectionString()))
                {
                    string query = "DetalleCuentaGetByIdCliente";
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = context;
                        cmd.CommandText = query;
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter[] collection = new SqlParameter[1];
                        collection[0] = new SqlParameter("IdCliente", SqlDbType.Int);
                        collection[0].Value = idCliente;
                        cmd.Parameters.AddRange(collection);
                        cmd.Connection.Open();

                        DataTable tableCliente = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                        adapter.Fill(tableCliente);

                        if (tableCliente.Rows.Count > 0)
                        {
                            DataRow row = tableCliente.Rows[0];
                            ML.DetalleCuenta detalleCuenta = new ML.DetalleCuenta();
                            detalleCuenta.IdDetalle = int.Parse(row[0].ToString());
                            detalleCuenta.Saldo = int.Parse(row[1].ToString());
                            detalleCuenta.Cliente = new ML.Cliente();
                            detalleCuenta.Cliente.IdCliente = int.Parse(row[2].ToString());
                            detalleCuenta.Cliente.Nombre = row[3].ToString();
                            detalleCuenta.Cliente.ApellidoPaterno = row[4].ToString();
                            detalleCuenta.Cliente.ApellidoMaterno = row[5].ToString();
                            detalleCuenta.Cliente.NumeroCuenta = int.Parse(row[6].ToString());


                            result.Object = detalleCuenta;
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = " No existen registros en la tabla";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
        public static ML.Result GetDenominacion()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (SqlConnection context = new SqlConnection(DL.Conexion.GetConnectionString()))
                {
                    string query = "DenominacionGetAll";
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = context;
                        cmd.CommandText = query;
                        cmd.CommandType = CommandType.StoredProcedure;


                        DataTable tableDenominacion = new DataTable();

                        SqlDataAdapter da = new SqlDataAdapter(cmd);

                        da.Fill(tableDenominacion);

                        if (tableDenominacion.Rows.Count > 0)
                        {
                            result.Objects = new List<object>();
                            foreach (DataRow row in tableDenominacion.Rows)
                            {

                                ML.Denominacion denominacion = new ML.Denominacion();
                                denominacion.IdDenominacion = int.Parse(row[0].ToString());
                                denominacion.Descripcion = row[1].ToString();
                                denominacion.Stock = int.Parse(row[2].ToString());
                                result.Objects.Add(denominacion);
                            }

                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = " No existen registros en la tabla";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
        public static ML.Result Retiro(int retiro, int saldo)
        {
            
            ML.Result result = new ML.Result();
            ML.Denominacion denominacion = new ML.Denominacion();
            ML.Result result1 = BL.Cliente.GetDenominacion();
            List<int> Stocks = new List<int>();
            int i = 0;
            if (result1.Correct)
            {
                foreach(ML.Denominacion billetes in result1.Objects)
                {
                    Stocks.Add(billetes.Stock);
                }
            }
            try
            {
                if (saldo >= retiro)
                {
                    if (retiro >= 1000)
                    {
                        denominacion.Billete1000 = retiro / 1000;
                        if (Stocks[0]> denominacion.Billete1000)
                        {
                            retiro = retiro - (denominacion.Billete1000 * 1000);
                        }
                        else
                        {
                            retiro = retiro;
                        }
                    }
                    if (retiro >= 500)
                    {
                        denominacion.Billete500 = retiro / 500;
                        if (Stocks[1] > denominacion.Billete500)
                        {
                            retiro = retiro - (denominacion.Billete500 * 500);
                        }
                        else
                        {
                            retiro = retiro;
                        }
                    }
                    if (retiro >= 200)
                    {
                        denominacion.Billete200 = retiro / 200;
                        if (Stocks[2] > denominacion.Billete200) {
                            retiro = retiro - (denominacion.Billete200 * 200);
                        }
                        else
                        {
                            retiro = retiro;

                        }
                    }
                    if (retiro >= 100)
                    {
                        denominacion.Billete100 = retiro / 100;
                        if (Stocks[3] > denominacion.Billete100)
                        {
                            retiro = retiro - (denominacion.Billete100 * 100);
                        }
                        else
                        {
                            retiro = retiro;

                        }
                    }
                    if (retiro >= 50)
                    {
                        denominacion.Billete50 = retiro / 50;
                        if (Stocks[4] > denominacion.Billete50)
                        {
                            retiro = retiro - (denominacion.Billete50 * 50);
                        }
                        else
                        {
                            retiro = retiro;

                        }
                    }
                                if (retiro >= 20)
                                {
                                    denominacion.Billete20 = retiro / 20;
                                    if (Stocks[5] > denominacion.Billete20)
                                    {
                                        retiro = retiro - (denominacion.Billete20 * 20);
                                    }
                                    else
                                    {
                                        retiro = retiro;

                                    }
                                }
                                if (retiro < 20)
                                {
                                    denominacion.Sobrante = retiro;
                                }

                                result.Object = denominacion;
                                result.Correct = true;
                            }
                            else
                            {
                                result.ErrorMessage = "La cantidad ingresada es mayor que el saldo actual ";
                                result.Correct = false;

                            }
                        } 
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;

            }
            return result;
        }
        public static ML.Result AddRetiro(int idCliente, int saldo,ML.Denominacion denominacion)
        {
            ML.Result result = new ML.Result();
            try
            {

                using (SqlConnection context = new SqlConnection(DL.Conexion.GetConnectionString()))
                {
                    string query = "RetiroAdd";
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = context;
                        cmd.CommandText = query;
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter[] collection = new SqlParameter[9];
                        collection[0] = new SqlParameter("IdCliente", SqlDbType.Int);
                        collection[0].Value = idCliente;
                        collection[1] = new SqlParameter("Cantidad", SqlDbType.Int);
                        collection[1].Value = saldo;
                        collection[2]= new SqlParameter("Billete20", SqlDbType.Int);
                        collection[2].Value = denominacion.Billete20;
                        collection[3] = new SqlParameter("Billete50", SqlDbType.Int);
                        collection[3].Value = denominacion.Billete50;
                        collection[4] = new SqlParameter("Billete100", SqlDbType.Int);
                        collection[4].Value = denominacion.Billete100;
                        collection[5] = new SqlParameter("Billete200", SqlDbType.Int);
                        collection[5].Value = denominacion.Billete200;
                        collection[6] = new SqlParameter("Billete500", SqlDbType.Int);
                        collection[6].Value = denominacion.Billete500;
                        collection[7] = new SqlParameter("Billete1000", SqlDbType.Int);
                        collection[7].Value = denominacion.Billete1000;
                        collection[8] = new SqlParameter("SaldoRetenido", SqlDbType.Int);
                        collection[8].Value = denominacion.Sobrante;
                        cmd.Parameters.AddRange(collection);

                        cmd.Connection.Open();

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            result.Correct = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex =ex;
            }
            return result;
        }

        public static ML.Result GetRetiroIdCliente(int idCliente)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (SqlConnection context = new SqlConnection(DL.Conexion.GetConnectionString()))
                {
                    string query = "GetRetiro";
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = context;
                        cmd.CommandText = query;
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter[] collection = new SqlParameter[1];
                        collection[0] = new SqlParameter("IdCliente", SqlDbType.Int);
                        collection[0].Value = idCliente;
                        cmd.Parameters.AddRange(collection);
                        cmd.Connection.Open();

                        DataTable tableCliente = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                        adapter.Fill(tableCliente);

                        if (tableCliente.Rows.Count > 0)
                        {
                            DataRow row = tableCliente.Rows[0];
                            ML.Retiro retiro = new ML.Retiro();
                            retiro.Cliente = new ML.Cliente();
                            retiro.Cliente.IdCliente = int.Parse(row[0].ToString());
                            retiro.Cliente.Nombre = row[1].ToString();
                            retiro.Cliente.ApellidoPaterno = row[2].ToString();
                            retiro.Cliente.ApellidoMaterno = row[3].ToString();
                            retiro.Cantidad = int.Parse(row[4].ToString());
                            retiro.Denominacion = new ML.Denominacion();
                            retiro.Denominacion.Billete20 = int.Parse(row[5].ToString());
                            retiro.Denominacion.Billete50 = int.Parse(row[6].ToString());
                            retiro.Denominacion.Billete100 = int.Parse(row[7].ToString());
                            retiro.Denominacion.Billete200 = int.Parse(row[8].ToString());
                            retiro.Denominacion.Billete500 = int.Parse(row[9].ToString());
                            retiro.Denominacion.Billete1000 = int.Parse(row[10].ToString());
                            retiro.SaldoRetenido = int.Parse(row[11].ToString());

                            result.Object = retiro;
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = " No existen registros en la tabla";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

    }
}