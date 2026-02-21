using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

namespace proyecto2.Backend
{
    public class clsConsultasServicios
    {
        public DataTable cargarServicios()
        {
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT claveServicio, nombreServicio FROM servicios", cn);

                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }

        }

        public decimal ObtenerPrecioServicio(int id)
        {
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {

                SqlCommand cmd = new SqlCommand(
                    "SELECT costoBase FROM servicios WHERE claveServicio=@id", cn);

                cmd.Parameters.AddWithValue("@id", id);

                cn.Open();
                decimal precio = Convert.ToDecimal(cmd.ExecuteScalar());
                cn.Close();

                return precio;
            }

        }
    }
}