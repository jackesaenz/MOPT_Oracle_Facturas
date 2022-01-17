using ModeloDatos.Facturas;
using ModeloDatos.Proveedor;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WS_UIF_ORACLE
{
    /// <summary>
    /// Descripción breve de WS_SINAEP
    /// </summary>
    [WebService(Namespace = "http://aplicaciones.mopt.go.cr/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class WS_SINAEP : System.Web.Services.WebService
    {
        Negocio.SINAEP sinaep = new Negocio.SINAEP();

        /// <summary>
        /// ELABORADO POR: UNIDAD INFORMATICA - FINANCIERO
        /// AUTOR: Jackeline Sáenz Sampson
        /// Consulta Facturas de Servicios Públicos
        /// </summary>
        [WebMethod]
        public void ConsultaServPublicos(string vFechaInicio, string vFechaFinal, string vServicio, string vNumFactura, string vEstadoFactura, string vProveedor, string vBoletaPago,
            string vAcuerdoPago, string vAnoAcuerdo, out string[] vDescMsjError, out List<ServPublicos> vListaFacturas)
        {
            vDescMsjError = new string[0];
            DateTime fecha;

            bool vError = false;
            int i = 0;
            int anoA;
            vListaFacturas = new List<ServPublicos>();
            ServPublicos factura = new ServPublicos();

            if (string.IsNullOrEmpty(vAcuerdoPago.Trim()) && string.IsNullOrEmpty(vAnoAcuerdo.Trim()) && string.IsNullOrEmpty(vFechaInicio.Trim()))
            {
                vError = true;
                Array.Resize(ref vDescMsjError, vDescMsjError.Length + 1);
                vDescMsjError[i] = "007-Se requiere un valor para la Fecha Inicio.";
                i++;
            }
            else if (!string.IsNullOrEmpty(vFechaInicio.Trim()))
            {
                if (DateTime.TryParseExact(vFechaInicio, "dd/MM/yyyy", new CultureInfo("es-MX"), DateTimeStyles.None, out fecha))
                {
                    string fechaInicio = DateTime.ParseExact(vFechaInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                    vFechaInicio = fechaInicio;
                }
                else
                {
                    vError = true;
                    Array.Resize(ref vDescMsjError, vDescMsjError.Length + 1);
                    vDescMsjError[i] = "001-Formato incorrecto para la Fecha Inicio (dd/mm/yyyy).";
                    i++;
                }
                
            }
            if (string.IsNullOrEmpty(vAcuerdoPago.Trim()) && string.IsNullOrEmpty(vAnoAcuerdo.Trim()) && string.IsNullOrEmpty(vFechaFinal.Trim()))
            {
                vError = true;
                Array.Resize(ref vDescMsjError, vDescMsjError.Length + 1);
                vDescMsjError[i] = "007-Se requiere un valor para la Fecha Final.";
                i++;
            }
            else if (!string.IsNullOrEmpty(vFechaFinal.Trim()))
            {
                if (DateTime.TryParseExact(vFechaFinal, "dd/MM/yyyy", new CultureInfo("es-MX"), DateTimeStyles.None, out fecha))
                {
                    string fechaFinal = DateTime.ParseExact(vFechaFinal, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                    vFechaFinal = fechaFinal;
                }
                else
                {
                    vError = true;
                    Array.Resize(ref vDescMsjError, vDescMsjError.Length + 1);
                    vDescMsjError[i] = "001-Formato incorrecto para la Fecha Final (dd/mm/yyyy).";
                    i++;
                }
            }
            if (!string.IsNullOrEmpty(vServicio.Trim()) && !vAnoAcuerdo.Equals("?"))
            {
               if (vServicio.ToUpper().Equals("AGUA"))
               {
                    vServicio = "10201";
               }
               if (vServicio.ToUpper().Substring(0,5).Equals("ELECT"))
               {
                    vServicio = "10202";
               }
                if (vServicio.ToUpper().Substring(0, 5).Equals("TELEF"))
                {
                    vServicio = "10204";
                }
                if (vServicio.ToUpper().Substring(0, 5).Equals("URBAN"))
                {
                    vServicio = "10299";
                }
            }
            if (!string.IsNullOrEmpty(vEstadoFactura.Trim()))
            {
                if (vEstadoFactura.ToUpper().Equals("ACTIVA"))
                {
                    vEstadoFactura = "01";
                }
                if (vEstadoFactura.ToUpper().Equals("ANULADA"))
                {
                    vEstadoFactura = "02";
                }
            }
            if (!string.IsNullOrEmpty(vAcuerdoPago.Trim()))
            {
                if (string.IsNullOrEmpty(vAnoAcuerdo.Trim()))
                {
                    vError = true;
                    Array.Resize(ref vDescMsjError, vDescMsjError.Length + 1);
                    vDescMsjError[i] = "007-Se requiere un valor para el Año del Acuerdo.";
                    i++;
                }
            }
            if (!string.IsNullOrEmpty(vAnoAcuerdo.Trim()) && !vAnoAcuerdo.Equals("?"))
            {
                if (string.IsNullOrEmpty(vAcuerdoPago.Trim()))
                {
                    vError = true;
                    Array.Resize(ref vDescMsjError, vDescMsjError.Length + 1);
                    vDescMsjError[i] = "007-Se requiere un valor para el número del Acuerdo Pago.";
                    i++;
                }
                else if (vAnoAcuerdo.Length != 4 || !(int.TryParse(vAnoAcuerdo, out anoA)))
                {
                    vError = true;
                    Array.Resize(ref vDescMsjError, vDescMsjError.Length + 1);
                    vDescMsjError[i] = "001-Formato incorrecto para el Año del acuerdo.";
                    i++;
                }
            }

            if (!vError)
            {
                sinaep.ConsultaServPublicos(vFechaInicio, vFechaFinal, vServicio, vNumFactura, vEstadoFactura, vProveedor, vBoletaPago, vAcuerdoPago, vAnoAcuerdo, out vDescMsjError, out vListaFacturas);
            }
                
        }

        /// <summary>
        /// ELABORADO POR: UNIDAD INFORMATICA - FINANCIERO
        /// AUTOR: Jackeline Sáenz Sampson
        /// Consulta Facturas de Ejecución Presupuestaria
        /// </summary>
        [WebMethod]
        public void ConsultaEjecPresupuestaria(string vFechaInicio, string vFechaFinal, string vNumFactura, string vCedulaJuridica, out string[] vDescMsjError,  out List<EjecPresupuestaria> vListaFacturas)
        {
            vDescMsjError = new string[0];
            DateTime fecha;

            bool vError = false;
            int i = 0;
            vListaFacturas = new List<EjecPresupuestaria>();

            if (string.IsNullOrEmpty(vFechaInicio.Trim()))
            {
                vError = true;
                Array.Resize(ref vDescMsjError, vDescMsjError.Length + 1);
                vDescMsjError[i] = "007-Se requiere un valor para la Fecha Inicio.";
                i++;
            }
            else if (!string.IsNullOrEmpty(vFechaInicio.Trim()))
            {
                if (DateTime.TryParseExact(vFechaInicio, "dd/MM/yyyy", new CultureInfo("es-MX"), DateTimeStyles.None, out fecha))
                {
                    string fechaInicio = DateTime.ParseExact(vFechaInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                    vFechaInicio = fechaInicio;
                }
                else
                {
                    vError = true;
                    Array.Resize(ref vDescMsjError, vDescMsjError.Length + 1);
                    vDescMsjError[i] = "001-Formato incorrecto para la Fecha Inicio (dd/mm/yyyy).";
                    i++;
                }

            }
            if (string.IsNullOrEmpty(vFechaFinal.Trim()))
            {
                vError = true;
                Array.Resize(ref vDescMsjError, vDescMsjError.Length + 1);
                vDescMsjError[i] = "007-Se requiere un valor para la Fecha Final.";
                i++;
            }
            else if (!string.IsNullOrEmpty(vFechaFinal.Trim()))
            {
                if (DateTime.TryParseExact(vFechaFinal, "dd/MM/yyyy", new CultureInfo("es-MX"), DateTimeStyles.None, out fecha))
                {
                    string fechaFinal = DateTime.ParseExact(vFechaFinal, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                    vFechaFinal = fechaFinal;
                }
                else
                {
                    vError = true;
                    Array.Resize(ref vDescMsjError, vDescMsjError.Length + 1);
                    vDescMsjError[i] = "001-Formato incorrecto para la Fecha Final (dd/mm/yyyy).";
                    i++;
                }
            }

            if (string.IsNullOrEmpty(vCedulaJuridica.Trim()) || vCedulaJuridica.Equals("?"))
            {
                vError = true;
                Array.Resize(ref vDescMsjError, vDescMsjError.Length + 1);
                vDescMsjError[i] = "007-Se requiere un valor para la Cédula Jurídica.";
                i++;
            }

            if (!vError)
            {
                sinaep.ConsultaEjecPresupuestaria(vFechaInicio, vFechaFinal, vNumFactura, vCedulaJuridica, out vDescMsjError, out  vListaFacturas);
            }

        }

        /// <summary>
        /// ELABORADO POR: UNIDAD INFORMATICA - FINANCIERO
        /// AUTOR: Jackeline Sáenz Sampson
        /// Consulta si el proveedor existe por cédula jurídica
        /// </summary>
        [WebMethod]
        public void ConsultaExisteProveedor(string vCedulaJuridica, out string[] vDescMsjError, out bool vExiste)
        {
            vDescMsjError = new string[0];
            vExiste = false;

            bool vError = false;
            int i = 0;

            if (string.IsNullOrEmpty(vCedulaJuridica.Trim()) || vCedulaJuridica.Equals("?"))
            {
                vError = true;
                Array.Resize(ref vDescMsjError, vDescMsjError.Length + 1);
                vDescMsjError[i] = "007-Se requiere un valor para la Cédula Jurídica del Proveedor.";
                i++;
            }

            if (!vError)
            {
                sinaep.ConsultaExisteProveedor(vCedulaJuridica, out vDescMsjError, out vExiste);
            }
        }

        /// <summary>
        /// ELABORADO POR: UNIDAD INFORMATICA - FINANCIERO
        /// AUTOR: Jackeline Sáenz Sampson
        /// Consulta proveedor por coincidencias de palabras en el nombre
        /// </summary>
        [WebMethod]
        public void ConsultaNombreProveedor(string vNombre, out string[] vDescMsjError, out List<Proveedores> vListaProveedores)
        {
            vDescMsjError = new string[0];
            vListaProveedores = new List<Proveedores>();

            bool vError = false;
            int i = 0;

            if (string.IsNullOrEmpty(vNombre.Trim()) || vNombre.Equals("?"))
            {
                vError = true;
                Array.Resize(ref vDescMsjError, vDescMsjError.Length + 1);
                vDescMsjError[i] = "007-Se requiere un valor para el nombre del Proveedor.";
                i++;
            }

            if (!vError)
            {
                sinaep.ConsultaNombreProveedor(vNombre, out vDescMsjError, out vListaProveedores);
            }
        }

        /// <summary>
        /// ELABORADO POR: UNIDAD INFORMATICA - FINANCIERO
        /// AUTOR: Jackeline Sáenz Sampson
        /// Consulta proveedor por número de cédula
        /// </summary>
        [WebMethod]
        public void ConsultaCedulaProveedor(string vCedula, out string[] vDescMsjError, out List<Proveedores> vListaProveedores)
        {
            vDescMsjError = new string[0];
            vListaProveedores = new List<Proveedores>();

            bool vError = false;
            int i = 0;

            if (string.IsNullOrEmpty(vCedula.Trim()) || vCedula.Equals("?"))
            {
                vError = true;
                Array.Resize(ref vDescMsjError, vDescMsjError.Length + 1);
                vDescMsjError[i] = "007-Se requiere un valor para la cédula del Proveedor.";
                i++;
            }

            if (!vError)
            {
                sinaep.ConsultaCedulaProveedor(vCedula, out vDescMsjError, out vListaProveedores);
            }
        }
    }
}
