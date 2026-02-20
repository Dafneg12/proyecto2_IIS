<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="proyecto2.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registro de Órdenes</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body class="bg-light">

<form id="form1" runat="server">

<div class="container mt-4">

    <!-- ENCABEZADO -->
    <div class="card shadow-sm mb-4">
        <div class="card-body text-white rounded" style="background:#A26493;">
            <h3 class="mb-0 text-center">Registro de Órdenes de Servicio</h3>
        </div>
    </div>

    <!-- DATOS GENERALES -->
    <div class="card shadow-sm mb-4">
        <div class="card-header fw-bold text-white" style="background:#38686E;">
            Datos Generales
        </div>
        <div class="card-body">
            <div class="row g-3">

                <div class="col-md-3">
                    <label>Fecha</label>
                    <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" ReadOnly="true"/>
                </div>

                <div class="col-md-3">
                    <label>Folio</label>
                    <asp:TextBox ID="txtFolio" runat="server" CssClass="form-control" ReadOnly="true"/>
                </div>

                <div class="col-md-6">
                    <label>Cliente</label>
                    <div class="input-group">
                        <asp:TextBox ID="txtCliente" runat="server" CssClass="form-control" ReadOnly="true"/>
                        <asp:Button ID="btnBuscarCliente" runat="server"
                            Text="Buscar"
                            CssClass="btn text-white"
                            Style="background:#A26493;"
                            OnClick="btnBuscarCliente_Click"/>
                    </div>
                </div>

                <div class="col-md-6">
                    <label>RFC</label>
                    <asp:TextBox ID="txtRFC" runat="server" CssClass="form-control" ReadOnly="true"/>
                </div>

                <div class="col-md-6">
                    <label>Vehículo</label>
                    <asp:DropDownList ID="ddlVehiculos" runat="server"
                        CssClass="form-select"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlVehiculos_SelectedIndexChanged" />
                </div>

                <div class="col-md-6">
                    <label>Descripción del vehículo</label>
                    <asp:TextBox ID="txtVehiculoDesc" runat="server"
                        CssClass="form-control"
                        ReadOnly="true"/>
                </div>

            </div>
        </div>
    </div>

    <!-- AGREGAR SERVICIOS -->
    <div class="card shadow-sm mb-4">
        <div class="card-header fw-bold text-white" style="background:#38686E;">
            Servicios
        </div>

        <div class="card-body">

            <div class="row g-3 align-items-end">

                <div class="col-md-6">
                    <label>Servicio</label>
                    <asp:DropDownList ID="ddlServicios" runat="server" CssClass="form-select"/>
                </div>

                <div class="col-md-3">
                    <label>Cantidad</label>
                    <asp:TextBox ID="txtCantidad" runat="server" CssClass="form-control"/>
                </div>

                <div class="col-md-3 d-grid">
                    <asp:Button ID="btnAgregar" runat="server"
                        Text="Agregar"
                        CssClass="btn text-white"
                        Style="background:#966A88;"
                        OnClick="btnAgregar_Click"/>
                </div>

            </div>

        </div>
    </div>

    <!-- TABLA DE DETALLE -->
    <div class="card shadow-sm mb-4">
        <div class="card-header fw-bold text-white" style="background:#38686E;">
            Detalle de Servicios
        </div>

        <div class="card-body">

            <asp:GridView ID="gvDetalle" runat="server"
                CssClass="table table-bordered table-hover text-center"
                AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="Servicio" HeaderText="Servicio" />
                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                    <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="{0:C}" />
                </Columns>
            </asp:GridView>

            <div class="row justify-content-end mt-3">
                <div class="col-md-4">
                    <label class="fw-bold">Total</label>
                    <asp:TextBox ID="txtTotal" runat="server"
                        CssClass="form-control form-control-lg text-end fw-bold"
                        ReadOnly="true"/>
                </div>
            </div>

        </div>
    </div>

    <!-- BOTON GUARDAR -->
    <div class="text-center mb-5">
        <asp:Button ID="btnGuardar" runat="server"
            Text="Guardar Orden"
            CssClass="btn btn-lg text-white px-5"
            Style="background:#966A88;"
            OnClick="btnGuardar_Click"/>
    </div>

</div>

</form>

</body>
</html>