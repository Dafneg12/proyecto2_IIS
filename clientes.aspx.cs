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
    public partial class clientes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarClientes();
            }
        }

        void CargarClientes()
        {
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                "SELECT claveCliente, rfc, nombre + ' ' + apellidoPaterno + ' ' + apellidoMaterno AS nombreCompleto FROM clientes",
                cn);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvClientes.DataSource = dt;
                gvClientes.DataBind();
            }
        }

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