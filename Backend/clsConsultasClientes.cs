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
    /// Clase que contiene métodos para realizar consultas relacionadas con los clientes y sus vehículos en la base de datos.
    /// Representa el backend para la gestión de clientes, permitiendo cargar la lista de clientes, obtener los vehículos asociados a un cliente específico y obtener detalles de un vehículo.
    /// </summary>
    public class clsConsultasClientes
    {
        /// <summary>
        /// Método que contiee la cosulta para cargar la lista de clientes desde la base de datos.
        /// Utiliza el using para asegurar que la conexión a la base de datos se cierre correctamente después de su uso.
        /// Regresa un DataTable con la información de los clientes, incluyendo su clave, RFC y nombre completo.
        /// </summary>
        public DataTable cargarClientes()
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT claveCliente, rfc, nombre + ' ' + apellidoPaterno + ' ' + apellidoMaterno AS nombreCompleto FROM clientes",
                cn);

                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// Método que contiene la consulta para cargar los vehículos asociados a un cliente específico, identificado por su clave (id).
        /// Utiliza el using para asegurar que la conexión a la base de datos se cierre correctamente después de su uso.
        /// Regresa un DataTable con la información de los vehículos, incluyendo su número de serie y una descripción que combina las placas, marca y modelo del vehículo.
        /// </summary>
        public DataTable cargarVehiculosCliente(int id)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(@"SELECT numeroSerie, placas + ' - ' + marca + ' ' + modelo AS vehiculo
                                                       FROM vehiculos WHERE claveCliente = @id", cn);

                da.SelectCommand.Parameters.AddWithValue("@id", id);

                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }

        /// <summary>
        /// Método que contiene la consulta para obtener los detalles de un vehículo específico, identificado por su número de serie (id).
        /// Utiliza el using para asegurar que la conexión a la base de datos se cierre correctamente después de su uso.
        /// Regresa una cadena de texto que combina la marca, modelo, color, placas y año del vehículo, proporcionando una descripción completa del vehículo.
        /// </summary>
        public string datosVehiculo(int id)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
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