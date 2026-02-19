using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace proyecto2
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Backend.clsConsultasClientes dt = new Backend.clsConsultasClientes();

                gvClientes.DataSource = dt.CargarClientes();
                gvClientes.DataBind();
            }
        }

        
    }
}