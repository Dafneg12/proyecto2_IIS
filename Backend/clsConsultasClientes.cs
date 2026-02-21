using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

namespace proyecto2.Backend
{
    public class clsConsultasClientes
    {

        public DataTable cargarClientes()
        {
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT claveCliente, rfc, nombre + ' ' + apellidoPaterno + ' ' + apellidoMaterno AS nombreCompleto FROM clientes",
                cn);

                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public DataTable cargarVehiculosCliente(int id)
        {
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(@"SELECT numeroSerie, placas + ' - ' + marca + ' ' + modelo AS vehiculo
                                                       FROM vehiculos WHERE claveCliente = @id", cn);

                da.SelectCommand.Parameters.AddWithValue("@id", id);

                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }

        public string datosVehiculo(int id)
        {
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"SELECT marca + ' ' + modelo + ' ' + color + ' - Placas: ' + placas + ' - Año: ' + CAST(anio AS varchar)
                                                FROM vehiculos WHERE numeroSerie = @id", cn);

                cmd.Parameters.AddWithValue("@id", id);

                cn.Open();
                return cmd.ExecuteScalar().ToString();
                
            }
        }
    }
}