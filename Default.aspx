<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="proyecto2.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Menú Principal</title>
    <style>
        body {
            font-family: Arial;
            background-color: #f4f6f9;
        }

        .contenedor {
            width: 600px;
            margin: 100px auto;
            background: white;
            padding: 30px;
            border-radius: 10px;
            text-align: center;
            box-shadow: 0 0 10px rgba(0,0,0,0.2);
        }

        h1 {
            margin-bottom: 25px;
        }

        .btn {
            width: 80%;
            padding: 15px;
            margin: 10px;
            font-size: 18px;
            border-radius: 8px;
            border: none;
            background: #007bff;
            color: white;
            cursor: pointer;
        }

        .btn:hover {
            background: #0056b3;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="contenedor">
            <h1>Taller Mecánico</h1>
            <h3>Menú Principal</h3>


            <asp:Button CssClass="btn" ID="btnOrden" runat="server"
                Text="Registrar Orden de Servicio" OnClick="btnOrden_Click" />

            <asp:Button CssClass="btn" ID="btnReportes" runat="server"
                Text="Reportes" OnClick="btnReportes_Click" />
        </div>
    </form>
</body>
</html>
