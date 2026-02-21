using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

namespace proyecto2.Backend
{
    /// <summary>
    /// Clase que contiene métodos para realizar consultas relacionadas con los servicios disponibles en la base de datos.
    /// </summary>
    public class clsConsultasServicios
    {
        /// <summary>
        /// Método que se encarga de obtener la lista de servicios disponibles desde la base de datos.
        /// Utiliza el using para asegurar que la conexión a la base de datos se cierre correctamente después de su uso.
        /// Regresa un DataTable con la información de los servicios, incluyendo su clave y nombre, que se puede utilizar para mostrar en el frontend.
        /// </summary>
        public DataTable cargarServicios()
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT claveServicio, nombreServicio FROM servicios", cn);

                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }

        }

        /// <summary>
        /// Método que se encarga de obtener el precio de un servicio específico, identificado por su clave (id).
        /// Utiliza el using para asegurar que la conexión a la base de datos se cierre correctamente después de su uso.
        /// Devuelve un decimal que representa el costo base del servicio.
        /// </summary>
        public decimal ObtenerPrecioServicio(int id)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {

                SqlCommand cmd = new SqlCommand("SELECT costoBase FROM servicios WHERE claveServicio=@id", cn);

                cmd.Parameters.AddWithValue("@id", id);

                cn.Open();
                decimal precio = Convert.ToDecimal(cmd.ExecuteScalar());
                cn.Close();

                return precio;
            }

        }
    }
}