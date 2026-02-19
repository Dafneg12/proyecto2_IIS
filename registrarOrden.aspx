<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="registrarOrden.aspx.cs" Inherits="proyecto2.registrarOrden" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Registro de Orden</title>
    <style>
        body { font-family: Arial; background: #f4f6f9; }
        .contenedor {
            width: 800px;
            margin: 40px auto;
            background: white;
            padding: 25px;
            border-radius: 10px;
        }
        h1 { text-align: center; }
        label { font-weight: bold; }
        .campo {
            width: 100%;
            padding: 8px;
            margin-bottom: 12px;
        }
    </style>
</head>
<body>
    <form runat="server">
        <div class="contenedor">
            <h1>Registro de Orden de Servicio</h1>

            <label>Cliente (Nombre):</label>
            <asp:DropDownList ID="ddlClientes" runat="server" CssClass="campo"
                AutoPostBack="true" OnSelectedIndexChanged="ddlClientes_SelectedIndexChanged">
            </asp:DropDownList>

            <label>RFC:</label>
            <asp:DropDownList ID="ddlRfc" runat="server" CssClass="campo"></asp:DropDownList>

            <label>Vehículo:</label>
            <asp:DropDownList ID="ddlVehiculos" runat="server" CssClass="campo"></asp:DropDownList>

            <label>Servicios:</label>
            <asp:CheckBoxList ID="cblServicios" runat="server"></asp:CheckBoxList>

            <br />
            <asp:Button ID="btnGuardar" runat="server" Text="Registrar Orden"
                OnClick="btnGuardar_Click" />
        </div>
    </form>
</body>
</html>
