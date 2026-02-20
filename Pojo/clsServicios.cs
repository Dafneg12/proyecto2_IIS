using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyecto2.Pojo
{
    public class clsServicios
    {
        public int claveServicio { get; set; }
        public string nombreServicio { get; set; }
        public string descripcion { get; set; }
        public float costoBase { get; set; }
        public string tiempoEstimado { get; set; }
    }
}