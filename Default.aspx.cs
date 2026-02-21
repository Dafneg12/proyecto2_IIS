using proyecto2.Backend;
using proyecto2.Pojo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace proyecto2
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFolio.Text = ObtenerSiguienteFolio().ToString();
                CargarServicios();
                txtFecha.Text = DateTime.Now.ToShortDateString();

                if (Session["cliente"] != null)
                {
                    txtCliente.Text = Session["cliente"].ToString();
                    txtRFC.Text = Session["rfc"].ToString();
                    CargarVehiculosCliente();
                }
            }
        }

        protected void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            Response.Redirect("Clientes.aspx");
        }

        void CargarServicios()
        {
            DataTable dt = new DataTable();
            clsConsultasServicios cons = new clsConsultasServicios();
            dt = cons.cargarServicios();

            ddlServicios.DataSource = dt;
            ddlServicios.DataTextField = "nombreServicio";
            ddlServicios.DataValueField = "claveServicio";
            ddlServicios.DataBind();

            ddlServicios.Items.Insert(0, new ListItem("--Seleccione--", "0"));
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            DataTable dt;

            if (Session["detalle"] == null)
            {
                dt = new DataTable();
                dt.Columns.Add("Servicio");
                dt.Columns.Add("Cantidad");
                dt.Columns.Add("Precio");
                dt.Columns.Add("Subtotal");
            }
            else
                dt = (DataTable)Session["detalle"];

            string servicio = ddlServicios.SelectedItem.Text;
            int cantidad = int.Parse(txtCantidad.Text);

            decimal precio = ObtenerPrecioServicio(
                int.Parse(ddlServicios.SelectedValue));

            decimal subtotal = precio * cantidad;

            dt.Rows.Add(servicio, cantidad, precio, subtotal);

            Session["detalle"] = dt;

            gvDetalle.DataSource = dt;
            gvDetalle.DataBind();

            CalcularTotal(dt);
        }

        decimal ObtenerPrecioServicio(int id)
        {
            clsConsultasServicios cons = new clsConsultasServicios();
            return cons.ObtenerPrecioServicio(id);
        }

        int ObtenerSiguienteFolio()
        {
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT ISNULL(MAX(folioOrden),0) + 1 FROM ordenesServicio", cn);

                cn.Open();
                int folio = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();

                return folio;
            }
        }

        void CargarVehiculosCliente()
        {
            if (Session["claveCliente"] == null) return;

            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    @"SELECT numeroSerie,
                     placas + ' - ' + marca + ' ' + modelo AS vehiculo
              FROM vehiculos
              WHERE claveCliente = @id", cn);

                da.SelectCommand.Parameters.AddWithValue("@id", Session["claveCliente"]);

                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlVehiculos.DataSource = dt;
                ddlVehiculos.DataTextField = "vehiculo";
                ddlVehiculos.DataValueField = "numeroSerie";
                ddlVehiculos.DataBind();

                ddlVehiculos.Items.Insert(0, new ListItem("--Seleccione--", "0"));
            }
        }

        protected void ddlVehiculos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlVehiculos.SelectedValue == "0")
            {
                txtVehiculoDesc.Text = "";
                return;
            }

            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"SELECT marca + ' ' + modelo + ' ' + color + 
                     ' - Placas: ' + placas + 
                     ' - Año: ' + CAST(anio AS varchar)
              FROM vehiculos
              WHERE numeroSerie = @id", cn);

                cmd.Parameters.AddWithValue("@id", ddlVehiculos.SelectedValue);

                cn.Open();
                txtVehiculoDesc.Text = cmd.ExecuteScalar().ToString();
                cn.Close();
            }
        }

        void CalcularTotal(DataTable dt)
        {
            decimal total = 0;

            foreach (DataRow row in dt.Rows)
            {
                total += Convert.ToDecimal(row["Subtotal"]);
            }

            txtTotal.Text = total.ToString("0.00");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Session["detalle"] == null || ddlVehiculos.SelectedValue == "0")
            {
                Response.Write("<script>alert('Debe seleccionar cliente, vehículo y al menos un servicio');</script>");
                return;
            }

            DataTable dt = (DataTable)Session["detalle"];

            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {
                cn.Open();
                SqlTransaction tran = cn.BeginTransaction();

                try
                {
                    // 1. Insertar ORDEN
                    SqlCommand cmdOrden = new SqlCommand(
                        @"INSERT INTO ordenesServicio
                  (fechaIngreso, fechaEstimadaEntrega, fechaRealEntrega, estado, costoTotal, numeroSerie)
                  VALUES (@fi, @fe, @fr, @estado, @total, @serie);
                  SELECT SCOPE_IDENTITY();", cn, tran);

                    cmdOrden.Parameters.AddWithValue("@fi", DateTime.Now);
                    cmdOrden.Parameters.AddWithValue("@fe", DateTime.Now.AddDays(2));
                    cmdOrden.Parameters.AddWithValue("@fr", DateTime.Now.AddDays(2));
                    cmdOrden.Parameters.AddWithValue("@estado", "Abierta");
                    cmdOrden.Parameters.AddWithValue("@total", Convert.ToDecimal(txtTotal.Text));
                    cmdOrden.Parameters.AddWithValue("@serie", ddlVehiculos.SelectedValue);

                    int folio = Convert.ToInt32(cmdOrden.ExecuteScalar());

                    // 2. Insertar DETALLE
                    foreach (DataRow row in dt.Rows)
                    {
                        SqlCommand cmdServicio = new SqlCommand(
                            @"INSERT INTO OrdenServicio (folioOrden, claveServicio)
                      VALUES (@folio, 
                      (SELECT claveServicio FROM servicios WHERE nombreServicio=@servicio))",
                            cn, tran);

                        cmdServicio.Parameters.AddWithValue("@folio", folio);
                        cmdServicio.Parameters.AddWithValue("@servicio", row["Servicio"].ToString());

                        cmdServicio.ExecuteNonQuery();
                    }

                    tran.Commit();

                    Response.Write("<script>alert('Orden registrada correctamente');window.location='Default.aspx';</script>");
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    Response.Write("<script>alert('Error al guardar: " + ex.Message + "');</script>");
                }
            }
        }

    }
}