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
    /// Clase que representa la página de clientes, donde se muestra una lista de clientes disponibles para seleccionar.
    /// </summary>
    public partial class clientes : System.Web.UI.Page
    {
        /// <summary>
        /// Método que se ejecuta al cargar la página. 
        /// Si la página se carga por primera vez (no es un postback), se llama al método CargarClientes para obtener y mostrar la lista de clientes en el GridView.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarClientes();
            }
        }

        /// <summary>
        /// Método que carga la lista de clientes desde la base de datos utilizando la clase clsConsultasClientes (Backend).
        /// </summary>
        void CargarClientes()
        {
            clsConsultasClientes consultas = new clsConsultasClientes();
            DataTable dt = consultas.cargarClientes();

            gvClientes.DataSource = dt;
            gvClientes.DataBind();

        }

        /// <summary>
        /// Método que se ejecuta cuando se selecciona un cliente en el GridView.
        /// Se obtienen los datos del cliente seleccionado y se almacenan en la sesión para mantener el estado del cliente seleccionado.
        /// Redirecciona a la página principal (Default.aspx) para mostrar los datos del cliente seleccionado y permitir la creación de órdenes de servicio.
        /// </summary>
        protected void gvClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int clave = Convert.ToInt32(gvClientes.DataKeys[gvClientes.SelectedIndex].Value);
            string nombre = gvClientes.SelectedRow.Cells[1].Text;
            string rfc = gvClientes.SelectedRow.Cells[2].Text;

            Session["claveCliente"] = clave;
            Session["cliente"] = nombre;
            Session["rfc"] = rfc;

            Response.Redirect("Default.aspx");
        }

    }
}