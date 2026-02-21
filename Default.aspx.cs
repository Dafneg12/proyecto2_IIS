using proyecto2.Backend;
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
            clsConsultasRegistroOrden cons = new clsConsultasRegistroOrden();
            return cons.ObtenerSiguienteFolio();
        }

        void CargarVehiculosCliente()
        {
            if (Session["claveCliente"] == null) return;

            DataTable dt = new DataTable();
            clsConsultasClientes cons = new clsConsultasClientes();
            dt = cons.cargarVehiculosCliente(Convert.ToInt32(Session["claveCliente"]));

            ddlVehiculos.DataSource = dt;
            ddlVehiculos.DataTextField = "vehiculo";
            ddlVehiculos.DataValueField = "numeroSerie";
            ddlVehiculos.DataBind();

            ddlVehiculos.Items.Insert(0, new ListItem("--Seleccione--", "0"));
        }

        protected void ddlVehiculos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlVehiculos.SelectedValue == "0")
            {
                txtVehiculoDesc.Text = "";
                return;
            }

            clsConsultasClientes cons = new clsConsultasClientes();

           txtVehiculoDesc.Text = cons.datosVehiculo(Convert.ToInt32(ddlVehiculos.SelectedValue)); ;
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

            clsConsultasRegistroOrden cons = new clsConsultasRegistroOrden();

            string resultado = cons.RegistrarOrden(int.Parse(txtFolio.Text), Convert.ToInt32(Session["claveCliente"]), ddlVehiculos.SelectedValue, DateTime.Now, 
                                Convert.ToDecimal(txtTotal.Text), dt);
            
            Response.Write(resultado);
        }

    }
}