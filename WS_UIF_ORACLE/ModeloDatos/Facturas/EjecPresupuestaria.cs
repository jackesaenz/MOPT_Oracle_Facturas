using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloDatos.Facturas
{
    public class EjecPresupuestaria
    {
        public string Programa { get; set; }

        public string Dependencia { get; set; }

        public string CedFactura { get; set; }

        //public string NomFactura { get; set; }

        public string FechaFactura { get; set; }

        public string NumFactura { get; set; }

        public string DescArticulo { get; set; }

        public decimal PrecioFactura { get; set; }

        public int Cantidad { get; set; }

        public decimal MontoFactura { get; set; }

        public decimal MontoIva { get; set; }

        public decimal Renta { get; set; }

        public decimal Multa { get; set; }

        public string Moneda { get; set; }

        public decimal TipoCambio { get; set; }

        public string DocPresupuestario { get; set; }

        public string CtaDeposito { get; set; }

        public string EstadoFactura { get; set; }

        public string NumAcuerdoPago { get; set; }

        public string FechaAcuerdoPago { get; set; }

        public string NumCesion { get; set; }

        public string NumCesionario { get; set; }

        //public string Cesionario { get; set; }
    }
}
