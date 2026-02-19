using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using proyecto2.Pojo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace proyecto2.Backend
{
    /// Clase para realizar consultas relacionadas con los empleados en la base de datos.
    public class clsConsultasClientes
    {
        public DataTable CargarClientes()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM clientes", cn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    return dt;

                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine("Error al cargar clientes: " + ex.Message);
                return null;
            }
        }
    }
}