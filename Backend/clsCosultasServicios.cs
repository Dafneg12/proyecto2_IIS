using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;

namespace proyecto2.Backend
{
    public class clsCosultasServicios
    {

        public DataTable cargarServicios()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter(
                        "SELECT claveServicio, nombreServicio, costoBase FROM servicios", cn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción (puedes registrar el error o mostrar un mensaje)
                throw new Exception("Error al cargar los servicios: " + ex.Message);
            }
        }

        
    }
}