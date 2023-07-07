using System;
using System.Configuration;
namespace DL
{
    public class Conexion
    {
        public static string GetConnectionString()
        {
            string cadenaConexion = "Data Source=.;Initial Catalog=LGracidaCajero;User ID=sa;Password=pass@word1;Connect Timeout=30;Encrypt=False;TrustServerCertificate=true;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            return cadenaConexion;
        }  
    }
}