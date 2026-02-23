using Azure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace proyecto2.Backend
{
    /// <summary>
    /// Clase que contiene métodos para realizar consultas relacionadas con el registro de órdenes de servicio en la base de datos.
    /// </summary>
    public class clsConsultasRegistroOrden
    {
        /// <summary>
        /// Método que contiene la consulta para obtener el siguiente folio disponible para una nueva orden de servicio.
        /// Utiliza el using para asegurar que la conexión a la base de datos se cierre correctamente después de su uso.
        /// Regresa un entero que representa el siguiente folio disponible, calculado como el máximo folio existente en la tabla de órdenes de servicio más uno.
        /// </summary>
        public int ObtenerSiguienteFolio()
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString)) 
            { 
                SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(folioOrden),0) + 1 FROM ordenesServicio", cn); 
                
                cn.Open(); 
                
                int folio = Convert.ToInt32(cmd.ExecuteScalar()); 
                
                cn.Close(); 
                
                return folio; 
            }
        }

        /// <summary>
        /// Método que contiene la lógica para registrar una nueva orden de servicio en la base de datos, incluyendo la inserción de los detalles de los servicios asociados a la orden.
        /// Utiliza el using para asegurar que la conexión a la base de datos se cierre correctamente después de su uso.
        /// Se utiliza una transacción para asegurar que todas las operaciones de inserción se realicen de manera atómica, garantizando la integridad de los datos.
        /// Regresa una cadena de texto que contiene un script de JavaScript para mostrar un mensaje de éxito o error al usuario, dependiendo del resultado de la operación de registro de la orden de servicio.
        /// </summary>
        public string RegistrarOrden(int folio, int claveCliente, string numeroSerie, DateTime fecha, decimal total, DataTable dt)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {
                cn.Open();
                SqlTransaction tran = cn.BeginTransaction();

                try
                {
                    SqlCommand cmdOrden = new SqlCommand( @"INSERT INTO ordenesServicio (fechaIngreso, fechaEstimadaEntrega, fechaRealEntrega, estado, costoTotal, numeroSerie)
                                                          VALUES (@fechaIngreso, @fechaEstimadaEntrega, @fechaEntrega, @estado, @total, @serie); SELECT SCOPE_IDENTITY();", cn, tran);

                    cmdOrden.Parameters.AddWithValue("@fechaIngreso", DateTime.Now);
                    cmdOrden.Parameters.AddWithValue("@fechaEstimadaEntrega", DateTime.Now.AddDays(2));
                    cmdOrden.Parameters.AddWithValue("@fechaEntrega", DateTime.Now.AddDays(2));
                    cmdOrden.Parameters.AddWithValue("@estado", "Abierta");
                    cmdOrden.Parameters.AddWithValue("@total", total);
                    cmdOrden.Parameters.AddWithValue("@serie", numeroSerie);

                    folio = Convert.ToInt32(cmdOrden.ExecuteScalar());

                    foreach (DataRow row in dt.Rows)
                    {
                        SqlCommand cmdServicio = new SqlCommand(@"INSERT INTO OrdenServicio (folioOrden, claveServicio)
                                                                VALUES (@folio, (SELECT claveServicio FROM servicios WHERE nombreServicio=@servicio))",cn, tran);

                        cmdServicio.Parameters.AddWithValue("@folio", folio);
                        cmdServicio.Parameters.AddWithValue("@servicio", row["Servicio"].ToString());

                        cmdServicio.ExecuteNonQuery();
                    }

                    tran.Commit();

                    return "<script>alert('Orden registrada correctamente');window.location='Default.aspx';</script>";
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return "<script>alert('Error al guardar: " + ex.Message + "');</script>";
                }
            }
        }
    }

}