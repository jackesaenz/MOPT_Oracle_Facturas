using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloDatos.Facturas
{
    public class ServPublicos
    {
        public string NumFactura  { get; set; }

        public string PeriodoFactura { get; set; }

        public string FechaFactura { get; set; }

        public decimal Monto { get; set; }

        public decimal ImpuestoRenta { get; set; }

        public decimal ImpuestoIva { get; set; }

        public decimal TributoBomberos { get; set; }

        public string Servicio { get; set; }

        public string NumBoletaPago { get; set; }

        public string DocHacienda { get; set; }

        public string NumAcuerdoPago { get; set; }

        public string FechaAcuerdoPago { get; set; }

        public string Proveedor { get; set; }

        public string FechaRecibido { get; set; }

        public List<DetServPublicos> Detalle { get; set; }

    }
}
