using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ProyectopProgra2
{
    public class ConexionBD
    {
        private static string cadena = @"Server=localhost\SQLEXPRESS;Database=Ferreteria_Ferre502;Trusted_Connection=True;";

        public static SqlConnection ObtenerConexion()
        {
            SqlConnection conexion = new SqlConnection(cadena);
            return conexion;
        }
    }
}
