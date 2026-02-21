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
    public class clsConsultasRegistroOrden
    {
        public int ObtenerSiguienteFolio()
        {
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(folioOrden),0) + 1 FROM ordenesServicio", cn);

                cn.Open();
                int folio = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();

                return folio;
            }
        }

        public string RegistrarOrden(int folio, int claveCliente, string numeroSerie, DateTime fecha, decimal total, DataTable dt)
        {
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
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