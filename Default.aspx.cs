using Microsoft.IdentityModel.Tokens;
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
    /// <summary>
    /// Pagina principal para registrar órdenes de servicio, seleccionar clientes, vehículos y servicios.
    /// </summary>
    public partial class Default : System.Web.UI.Page
    {
        /// <summary>
        /// Evento que se ejecuta al cargar la página. Inicializa datos cuando la página se carga por primera vez.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            /// <summary>
            /// condicion para verificar si la página se está cargando por primera vez o es una recarga (postback). 
            /// Si no es un postback, se inicializan los campos y se cargan los servicios.
            /// </summary>
            if (!IsPostBack)
            {
                txtFolio.Text = ObtenerSiguienteFolio().ToString();
                CargarServicios();
                txtFecha.Text = DateTime.Now.ToShortDateString();

                /// <summary>
                /// Condición para verificar si hay un cliente seleccionado en la sesión. Si es así, se cargan los datos del cliente y sus vehículos.
                /// La sesión la utilizamos para mantener el estado del cliente seleccionado entre diferentes páginas o recargas de la página actual.
                /// </summary>
                if (Session["cliente"] != null)
                {
                    txtCliente.Text = Session["cliente"].ToString();
                    txtRFC.Text = Session["rfc"].ToString();
                    CargarVehiculosCliente();
                }
            }
        }

        /// <summary>
        /// Metodo que se ejecuta al hacer clic en el botón "Buscar Cliente". Redirige a la página de clientes para seleccionar un cliente.
        /// </summary>
        protected void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            Response.Redirect("Clientes.aspx");
        }

        /// <summary>
        /// Método que obteniene la lista de servicios disponibles desde la base de datos y los carga en el dropdown list para que el usuario pueda seleccionarlos.
        /// También agrega una opción por defecto para indicar que el usuario debe seleccionar un servicio.
        /// Accede a la cosulta de servicios a través de la clase clsConsultasServicios (Backend).
        /// </summary>
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

        /// <summary>
        /// Método que agrega un servicio seleccionado al detalle de la orden. 
        /// Verifica que se haya seleccionado un servicio y especificado una cantidad válida.
        /// Utiliza la condicional de sesión para mantener el detalle de los servicios agregados entre recargas de la página.
        /// Calcula el subtotal de cada servicio y actualiza el total de la orden.
        /// </summary>
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

            if (ddlServicios.SelectedValue == "0" || string.IsNullOrEmpty(txtCantidad.Text))
            {
                Response.Write("<script>alert('Debe seleccionar un servicio y especificar la cantidad');</script>");
                return;
            }
            string servicio = ddlServicios.SelectedItem.Text;

            int cantidad = int.Parse(txtCantidad.Text);
            if (cantidad <= 0)
            {
                Response.Write("<script>alert('La cantidad debe ser un número entero positivo');</script>");
                return;
            }

            decimal precio = ObtenerPrecioServicio(
                int.Parse(ddlServicios.SelectedValue));

            decimal subtotal = precio * cantidad;

            dt.Rows.Add(servicio, cantidad, precio, subtotal);

            Session["detalle"] = dt;

            gvDetalle.DataSource = dt;
            gvDetalle.DataBind();

            CalcularTotal(dt);
        }

        /// <summary>
        /// Método que obtiene el precio de un servicio específico desde la base de datos utilizando la clase clsConsultasServicios.
        /// </summary>
        decimal ObtenerPrecioServicio(int id)
        {
            clsConsultasServicios cons = new clsConsultasServicios();
            return cons.ObtenerPrecioServicio(id);
        }

        /// <summary>
        /// Método que obtiene el siguiente folio disponible para una nueva orden de servicio desde la base de datos utilizando la clase clsConsultasRegistroOrden.
        /// </summary>
        int ObtenerSiguienteFolio()
        {
            clsConsultasRegistroOrden cons = new clsConsultasRegistroOrden();
            return cons.ObtenerSiguienteFolio();
        }

        /// <summary>
        /// Método que carga los vehículos asociados al cliente seleccionado en la sesión.
        /// El session se utiliza para mantener el estado del cliente seleccionado entre diferentes páginas o recargas de la página actual.
        /// Se utiliza la clase clsConsultasClientes para obtener los vehículos del cliente desde la base de datos y cargarlos en el dropdown list para que el usuario pueda seleccionarlos.
        /// </summary>
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

        /// <summary>
        /// Método que se ejecuta al cambiar la selección del vehículo en el dropdown list.
        /// Obtiene y muestra la descripción del vehículo seleccionado utilizando la clase clsConsultasClientes para acceder a los datos del vehículo desde la base de datos.
        /// </summary>
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

        /// <summary>
        /// Método que calcula el total de la orden sumando los subtotales de cada servicio agregado al detalle.
        /// </summary>
        void CalcularTotal(DataTable dt)
        {
            decimal total = 0;

            foreach (DataRow row in dt.Rows)
            {
                total += Convert.ToDecimal(row["Subtotal"]);
            }

            txtTotal.Text = total.ToString("0.00");
        }

        /// <summary>
        /// Método que encarga de guardar la orden de servicio en la base de datos cuando el usuario hace clic en el botón "Guardar Orden".
        /// La condicional de sesión se utiliza para verificar que se haya seleccionado un cliente, vehículo y agregado al menos un servicio antes de intentar guardar la orden.
        /// Se utiliza la clase clsConsultasRegistroOrden para registrar la orden en la base de datos, pasando todos los detalles necesarios como el folio, clave del cliente, número de serie del vehículo, fecha, total y el detalle de los servicios.
        /// Se muestra un mensaje de resultado al usuario después de intentar guardar la orden, indicando si fue exitoso o si hubo algún error.
        /// </summary>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Session["detalle"] == null || ddlVehiculos.SelectedValue == "0" || ddlServicios.SelectedValue == "0")
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