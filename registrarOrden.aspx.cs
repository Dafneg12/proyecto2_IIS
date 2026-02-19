using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace proyecto2
{
    public partial class registrarOrden : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarClientes();
                CargarServicios();
            }
        }

        void CargarClientes()
        {
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT claveCliente, nombre + ' ' + apellidoPaterno AS nombre FROM clientes", cn);

                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlClientes.DataSource = dt;
                ddlClientes.DataTextField = "nombre";
                ddlClientes.DataValueField = "claveCliente";
                ddlClientes.DataBind();

                ddlClientes.Items.Insert(0, "-- Seleccione --");
            }
        }

        void CargarRFC(int idCliente)
        {
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT rfc FROM clientes WHERE claveCliente=@id", cn);

                cmd.Parameters.AddWithValue("@id", idCliente);

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                ddlRfc.Items.Clear();

                if (dr.Read())
                {
                    ddlRfc.Items.Add(dr["rfc"].ToString());
                }
            }
        }

        void CargarVehiculos(int idCliente)
        {
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT numeroSerie, placas FROM vehiculos WHERE claveCliente=@id", cn);

                da.SelectCommand.Parameters.AddWithValue("@id", idCliente);

                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlVehiculos.DataSource = dt;
                ddlVehiculos.DataTextField = "placas";
                ddlVehiculos.DataValueField = "numeroSerie";
                ddlVehiculos.DataBind();
            }
        }

        void CargarServicios()
        {
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT claveServicio, nombreServicio FROM servicios", cn);

                DataTable dt = new DataTable();
                da.Fill(dt);

                cblServicios.DataSource = dt;
                cblServicios.DataTextField = "nombreServicio";
                cblServicios.DataValueField = "claveServicio";
                cblServicios.DataBind();
            }
        }

        protected void ddlClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idCliente = Convert.ToInt32(ddlClientes.SelectedValue);

            CargarRFC(idCliente);
            CargarVehiculos(idCliente);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO ordenesServicio
                    (fechaIngreso,fechaEstimadaEntrega,fechaRealEntrega,estado,costoTotal,numeroSerie)
                    VALUES(GETDATE(),DATEADD(day,2,GETDATE()),DATEADD(day,2,GETDATE()),'Abierta',0,@serie);
                    SELECT SCOPE_IDENTITY();", cn);

                cmd.Parameters.AddWithValue("@serie", ddlVehiculos.SelectedValue);

                int folio = Convert.ToInt32(cmd.ExecuteScalar());

                foreach (System.Web.UI.WebControls.ListItem item in cblServicios.Items)
                {
                    if (item.Selected)
                    {
                        SqlCommand cmd2 = new SqlCommand(
                            "INSERT INTO OrdenServicio VALUES(@f,@s)", cn);

                        cmd2.Parameters.AddWithValue("@f", folio);
                        cmd2.Parameters.AddWithValue("@s", item.Value);
                        cmd2.ExecuteNonQuery();
                    }
                }
            }

            Response.Write("<script>alert('Orden registrada correctamente');</script>");
        }
    }
}