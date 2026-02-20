<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="clientes.aspx.cs" Inherits="proyecto2.clientes" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Clientes</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .btn-elegir {
            background-color: #A26493;
            border-color: #A26493;
            color: white;
        }

        .btn-elegir:hover {
            background-color: #8d4f7a;
            border-color: #8d4f7a;
            color: white;
        }
    </style>
</head>

<body class="bg-light">

<form id="form1" runat="server">

<div class="container mt-4">

    <!-- ENCABEZADO -->
    <div class="card shadow-sm mb-4">
        <div class="card-body text-white rounded" style="background-color:#38686E;">
            <h3 class="mb-0 text-center">Seleccionar Cliente</h3>
        </div>
    </div>

    <!-- TABLA -->
    <div class="card shadow-sm">
        <div class="card-body">

            <asp:GridView ID="gvClientes" runat="server"
                CssClass="table table-bordered table-hover text-center"
                AutoGenerateColumns="false"
                DataKeyNames="claveCliente"
                OnSelectedIndexChanged="gvClientes_SelectedIndexChanged">

                <Columns>

                    <asp:CommandField ShowSelectButton="true"
                        ButtonType="Button"
                        SelectText="Elegir"
                        ControlStyle-CssClass="btn btn-sm btn-elegir" />

                    <asp:BoundField DataField="nombreCompleto" HeaderText="Cliente" />
                    <asp:BoundField DataField="rfc" HeaderText="RFC" />

                </Columns>

            </asp:GridView>

        </div>
    </div>

</div>

</form>

</body>
</html>