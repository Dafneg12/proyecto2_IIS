using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace proyecto2.Pojo
{
    /// Clase POJO.
    public class clsClienntes
    {
        /// Atributos de la clase clsAuditoria
        /// getters y setters de cada atributo
        public int claveCliente { get; set; }
        public string rfc { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string domicilio { get; set; }
        public string colonia { get; set; }
        public string codigoPostal { get; set; }
        public string ciudad { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
    }
}
