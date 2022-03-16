using ModeloDatos.Facturas;
using ModeloDatos.Proveedor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class SINAEP
    {
        BaseDatos.Oracle DBOracle = new BaseDatos.Oracle();
        private Criptografia.Desencripta vCriptografia = new Criptografia.Desencripta();

        /// <summary>
        /// ELABORADO POR: UNIDAD INFORMATICA - FINANCIERO
        /// AUTOR: Jackeline Sáenz Sampson
        /// Consulta Facturas de Servicios Públicos
        /// </summary>
        public void ConsultaServPublicos(string vFechaInicio, string vFechaFinal, string vServicio, string vNumFactura, string vEstadoFactura, string vProveedor, string vBoletaPago,
          string vAcuerdoPago, string vAnoAcuerdo, out string[] vDescMsjError, out List<ServPublicos> vListaFacturas)
        {
            vDescMsjError = new string[0];
            string msjError = "";
            vListaFacturas = new List<ServPublicos>();
            string sql = "";

            try
            {
                // Consulta Facturas Servicios Públicos (Acuerdo Pago)
                if (!string.IsNullOrEmpty(vAcuerdoPago.Trim()) && !string.IsNullOrEmpty(vAnoAcuerdo.Trim()))
                {
                    sql = @"SELECT sp.ID_FACTURA, sp.NRO_FACTURA, sp.PERIODO_COBRO,
                                    to_char(to_date(sp.FECHA_FACTURA, 'yyyymmdd'), 'dd/mm/yyyy') AS FECHA_FACTURA,
                                    NVL(sp.MONTO_FACTURA,0) AS MONTO_FACTURA, 
                                    NVL(sp.IMPUESTO_RENTA,0) AS IMPUESTO_RENTA, 
                                    NVL(sp.TRIBUTO_BOMBEROS,0) AS TRIBUTO_BOMBEROS, 
                                    NVL(sp.IMPUESTO_IVA,0) AS IMPUESTO_IVA,
                                    CASE bp.PARTIDA
                                          WHEN '10201' THEN 'AGUA'
                                          WHEN '10202' THEN 'ELECT'
                                          WHEN '10204' THEN 'TELEF'
                                          WHEN '10299' THEN 'URBAN'
                                    END AS SERVICIO,
                                    bp.DOCUMENTO AS NUM_BOLETA_PAGO,
                                    bp.DOCUMENTO_HDA,
                                    bp.NRO_ACUERDO,
                                    to_char(to_date(bp.FECHA_ACUERDO, 'yyyymmdd'), 'dd/mm/yyyy') AS FECHA_ACUERDO,
                                    sp.PROVEEDOR_SERVICIO AS PROVEEDOR,
                                    to_char(to_date(sp.FECHA_RECIBIDO, 'yyyymmdd'), 'dd/mm/yyyy') AS FECHA_RECIBIDO
                            FROM SINAEP_BOLETA_PAGO bp
                            INNER JOIN SINAEP_DET_PROC_SERV_PUB sp ON sp.BOLETA_PAGO = bp.DOCUMENTO
                            AND sp.ANO_PRESUPUESTARIO = bp.ANO
                            INNER JOIN SINAEP_DET_PROC_SERV_PUB sp ON bp.DOCUMENTO = sp.BOLETA_PAGO AND bp.PROGRAMA = sp.PROGRAMA
                            AND bp.DIRECCION = sp.SUBPROGRAMA AND bp.PARTIDA = sp.PARTIDA AND bp.OBRA = sp.OBRA AND bp.FTE_FINANC = sp.FTE_FINANCIAMIENTO
                            AND bp.ANO = sp.ANO_PRESUPUESTARIO
                            WHERE bp.TRANSACCION = '11' AND bp.PARTIDA IN ('10201','10202','10204','10299')
                            AND bp.NRO_ACUERDO = '" + vAcuerdoPago + "' AND bp.ANO = '" + vAnoAcuerdo + "' AND sp.PARTIDA IN NVL('" + vServicio + "',sp.PARTIDA)"
                           + "AND sp.NRO_FACTURA IN NVL('" + vNumFactura + "', sp.NRO_FACTURA) AND sp.PROVEEDOR_SERVICIO IN NVL('" + vProveedor + "', sp.PROVEEDOR_SERVICIO)"
                           + "AND sp.ESTADO IN NVL('" + vEstadoFactura + "',sp.ESTADO) AND sp.BOLETA_PAGO IN NVL('" + vBoletaPago + "',sp.BOLETA_PAGO)"
                           + "AND sp.FECHA_FACTURA >= NVL('" + vFechaInicio + "', sp.FECHA_FACTURA) AND sp.FECHA_FACTURA <= NVL('" + vFechaFinal + "', sp.FECHA_FACTURA)";
                }
                // Consulta Facturas Servicios Públicos (Otros Parámetros)
                else
                {
                    sql = @"SELECT sp.ID_FACTURA,sp.NRO_FACTURA, sp.PERIODO_COBRO, 
                                    to_char(to_date(sp.FECHA_FACTURA, 'yyyymmdd'), 'dd/mm/yyyy') AS FECHA_FACTURA,
                                    NVL(sp.MONTO_FACTURA,0) AS MONTO_FACTURA, 
                                    NVL(sp.IMPUESTO_RENTA,0) AS IMPUESTO_RENTA, 
                                    NVL(sp.TRIBUTO_BOMBEROS,0) AS TRIBUTO_BOMBEROS, 
                                    NVL(sp.IMPUESTO_IVA,0) AS IMPUESTO_IVA,
                                    CASE sp.PARTIDA
                                          WHEN '10201' THEN 'AGUA'
                                          WHEN '10202' THEN 'ELECT'
                                          WHEN '10204' THEN 'TELEF'
                                          WHEN '10299' THEN 'URBAN'
                                    END AS SERVICIO, 
                                    bp.DOCUMENTO AS NUM_BOLETA_PAGO, bp.DOCUMENTO_HDA,
                                    bp.NRO_ACUERDO,
                                    to_char(to_date(bp.FECHA_ACUERDO, 'yyyymmdd'), 'dd/mm/yyyy') AS FECHA_ACUERDO,
                                    sp.PROVEEDOR_SERVICIO AS PROVEEDOR,
                                    to_char(to_date(sp.FECHA_RECIBIDO, 'yyyymmdd'), 'dd/mm/yyyy') AS FECHA_RECIBIDO
                             FROM SINAEP_DET_PROC_SERV_PUB sp 
                             LEFT JOIN SINAEP_BOLETA_PAGO bp ON bp.DOCUMENTO = sp.BOLETA_PAGO AND bp.PROGRAMA = sp.PROGRAMA
                             AND bp.DIRECCION = sp.SUBPROGRAMA AND bp.PARTIDA = sp.PARTIDA AND bp.OBRA = sp.OBRA AND bp.FTE_FINANC = sp.FTE_FINANCIAMIENTO
                             AND bp.ANO = sp.ANO_PRESUPUESTARIO AND bp.TRANSACCION = '11'
                             WHERE (sp.FECHA_FACTURA >= '" + vFechaInicio + "' AND sp.FECHA_FACTURA <= '" + vFechaFinal + "') AND sp.PARTIDA IN NVL('" + vServicio + "',sp.PARTIDA)"
                            + "AND sp.NRO_FACTURA IN NVL('" + vNumFactura + "',sp.NRO_FACTURA) AND sp.PROVEEDOR_SERVICIO IN NVL('" + vProveedor + "',sp.PROVEEDOR_SERVICIO)"
                            + "AND sp.ESTADO IN NVL('" + vEstadoFactura + "',sp.ESTADO) AND sp.BOLETA_PAGO IN NVL('" + vBoletaPago + "',sp.BOLETA_PAGO) ";
                }

 
                DataSet dsServPub = DBOracle.Consultar(sql);
                List<ServPublicos>  ListaFacturas = new List<ServPublicos>();

                if (dsServPub != null && dsServPub.Tables.Count > 0 && dsServPub.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsServPub.Tables[0].Rows)
                    {
                        ServPublicos factura = new ServPublicos();
                        factura.NumFactura = row["NRO_FACTURA"].ToString();
                        factura.PeriodoFactura = row["PERIODO_COBRO"].ToString();
                        factura.FechaFactura = row["FECHA_FACTURA"].ToString();
                        factura.Monto = decimal.Parse(row["MONTO_FACTURA"].ToString());
                        factura.ImpuestoRenta = decimal.Parse(row["IMPUESTO_RENTA"].ToString());
                        factura.TributoBomberos = decimal.Parse(row["TRIBUTO_BOMBEROS"].ToString());
                        factura.ImpuestoIva = decimal.Parse(row["IMPUESTO_IVA"].ToString());
                        factura.Servicio = row["SERVICIO"].ToString();
                        factura.NumBoletaPago = row["NUM_BOLETA_PAGO"].ToString();
                        factura.DocHacienda = row["DOCUMENTO_HDA"].ToString();
                        factura.NumAcuerdoPago = row["NRO_ACUERDO"].ToString();
                        factura.FechaAcuerdoPago = row["FECHA_ACUERDO"].ToString();
                        factura.Proveedor = row["PROVEEDOR"].ToString();
                        factura.FechaRecibido = row["FECHA_RECIBIDO"].ToString();

                        // Obtiene el detalle de la factura
                        string sql2 = @"SELECT sp.PROGRAMA || '-' || sp.SUBPROGRAMA AS PROGRAMA,
                                           sp.AREA || '-' || sp.ACTIVIDAD AS AREA_ACTIVIDAD,
                                           df.ID_REFERENCIA,df.CANTIDAD_CONSUMO,
                                           df.MONTO_ID_REFERENCIA,df.SECCION
                                    FROM SINAEP_DET_FACT_SERV_PUBL df 
                                    INNER JOIN SINAEP_DET_PROC_SERV_PUB sp ON sp.ID_FACTURA = df.ID_FACTURA
                                    AND sp.NRO_FACTURA = df.NRO_FACTURA
                                    WHERE df.ID_FACTURA = '" + row["ID_FACTURA"].ToString() + "' AND df.NRO_FACTURA = '" + row["NRO_FACTURA"].ToString() + "'";
                        //+ "' AND df.ESTADO = '01' ";

                        DataSet dtDetServPub = DBOracle.Consultar(sql2);
                        List<DetServPublicos> ListaDetalle = new List<DetServPublicos>();
                        foreach (DataRow row2 in dtDetServPub.Tables[0].Rows)
                        {
                            DetServPublicos detalle = new DetServPublicos();
                            detalle.Programa = row2["PROGRAMA"].ToString();
                            detalle.AreaActividad = row2["AREA_ACTIVIDAD"].ToString();
                            detalle.IdReferencia = row2["ID_REFERENCIA"].ToString();
                            detalle.Consumo = row2["CANTIDAD_CONSUMO"].ToString();
                            detalle.MontoReferencia = row2["MONTO_ID_REFERENCIA"].ToString();
                            detalle.CentroCosto = row2["SECCION"].ToString();
                            ListaDetalle.Add(detalle);
                        }
                        factura.Detalle = ListaDetalle;
                        ListaFacturas.Add(factura);
                    }
                    vListaFacturas = ListaFacturas;
                }
            }
            catch (Exception ex)
            {
                Array.Resize(ref vDescMsjError, vDescMsjError.Length + 1);
                vDescMsjError[0] = msjError;
            }
        }

        /// <summary>
        /// ELABORADO POR: UNIDAD INFORMATICA - FINANCIERO
        /// AUTOR: Jackeline Sáenz Sampson
        /// Consulta Facturas de Ejecución Presupuestaria
        /// </summary>
        public void ConsultaEjecPresupuestaria(string vFechaInicio, string vFechaFinal, string vNumFactura, string vCedulaJuridica, out string[] vDescMsjError, out List<EjecPresupuestaria> vListaFacturas)
        {
            vDescMsjError = new string[0];
            string msjError = "";
            vListaFacturas = new List<EjecPresupuestaria>();
            
            try
            {
                string sql = @" SELECT bpp.PROGRAMA ||'-'|| bpp.SUB_PROGRAMA AS PROGRAMA, 
                                       bpp.NRO_FACTURA_COMERCIAL AS NUM_FACTURA,
                                       bpp.DETALLE_ARTICULO AS DESC_ARTICULO,
                                       to_char(to_date(bpp.FECHA_FACTURA_COMERCIAL, 'yyyymmdd'), 'dd/mm/yyyy') AS FECHA_FACTURA_COMERCIAL,
                                       CASE bpp.ESTADO_TRAMITE
                                              WHEN '01' THEN '01-REGISTRO PRE-ENTRADA'
                                              WHEN '02' THEN '02-REVISIÓN BODEGA'
                                              WHEN '03' THEN '03-VISADO TÉCNICO'
                                              WHEN '04' THEN '04-AUTORIZACION-EJECUTOR'
                                              WHEN '04' THEN '04-AUTORIZACION-EJECUTOR'
                                              WHEN '05' THEN '05-REGISTRO ENTRADA'
                                              WHEN '06' THEN '06-REGISTRO MIGO'
                                              WHEN '07' THEN '07-APLICACION BOLETA PAGO'
                                              WHEN '08' THEN '08-APLICACION ACUERDO PAGO'
                                              WHEN '09' THEN '09-ANULACIÓN BOLETA PAGO'
                                              WHEN '10' THEN '10-DESAPLICAR ACUERDO PAGO'
                                       END AS ESTADO_FACTURA,
                                       bpp.CEDULA_PROVEEDOR AS CEDULA_FACTURA,
                                       bpp.RESPALDO AS DOCUMENTO_PRESUPUESTARIO,
                                       bpp.CANTIDAD AS CANTIDAD,
                                       bpp.PRECIO AS PRECIO_FACTURA,
                                       bpp.MONTO AS MONTO_FACTURA,
                                       bpp.IMPUESTO_IVA AS MONTO_IVA,
                                       bpp.IMPUESTO_RENTA AS RENTA,
                                       bpp.MONTO_MULTA AS MULTA,
                                       --bpp.BANCO_SINPE, bpp.CUENTA_SINPE, bpp.CONTROL_SINPE,
                                       bpp.BANCO_SINPE || bpp.CUENTA_SINPE || bpp.CONTROL_SINPE AS CUENTA_DEPOSITO,
                                       DECODE(bpp.NRO_ACUERDO_PAGO,'SN','SN',bpp.NRO_ACUERDO_PAGO) AS NUMERO_ACUERDO_PAGO,
                                       DECODE(NVL(bpp.FECHA_ACUERDO_PAGO,' '),' ','',to_char(to_date(bpp.FECHA_ACUERDO_PAGO, 'yyyymmdd'), 'dd/mm/yyyy')) AS FECHA_ACUERDO_PAGO,
                                       (select oc.SECCIONES || '-' || sec.DESCRIPCION AS DEPENDENCIA
                                FROM SINAEP_BOL_PAGO_PROV bpp1
                                INNER JOIN SINAEP_ORDEN_COMPRA oc ON oc.ANO = bpp1.ANO AND oc.TRANSACCION = 'OP' AND oc.DOCUMENTO = bpp1.RESPALDO
                                INNER JOIN sinaep_rel_estruct_prog_org sec ON sec.SECCION = oc.SECCIONES
                                WHERE bpp1.ANO = bpp.ANO AND bpp1.TRANSACCION = bpp.TRANSACCION AND bpp1.RESPALDO = bpp.RESPALDO AND bpp1.NRO_FACTURA_COMERCIAL = bpp.NRO_FACTURA_COMERCIAL
                                UNION
                                select c.CENTRO_COSTO || '-' || sec.DESCRIPCION AS DEPENDENCIA
                                FROM SINAEP_BOL_PAGO_PROV bpp2
                                INNER JOIN SINAEP_RESERV_RECURSOS rr ON rr.ANO = bpp2.ANO AND rr.TRANSACCION = '09' AND rr.DOCUMENTO = bpp2.RESPALDO 
                                INNER JOIN ADPRF01D09_UNI_EJECUTORAS ueje ON ueje.ANO = rr.ANO AND ueje.DOCUMENTO = rr.DOCUMENTO AND ueje.TRANSACCION = '09'
                                INNER JOIN ADPRF01D09_CENTRO_COSTO c ON c.ANO = ueje.ANO AND c.DOCUMENTO = ueje.DOCUMENTO AND c.TRANSACCION = '09' AND c.ID_U = ueje.ID_U
                                INNER JOIN sinaep_rel_estruct_prog_org sec ON sec.SECCION = c.CENTRO_COSTO
                                WHERE bpp2.ANO = bpp.ANO AND bpp2.TRANSACCION = bpp.TRANSACCION AND bpp2.RESPALDO = bpp.RESPALDO AND bpp2.NRO_FACTURA_COMERCIAL = bpp.NRO_FACTURA_COMERCIAL) AS DEPENDENCIA,
                                CASE bpp.CODIGO_MONEDA 
									WHEN '1' THEN 'COLONES'
									WHEN '2' THEN 'DOLARES'
								END AS MONEDA,
                                bpp.TIPO_CAMBIO,
                                bpp.NUMERO_CONTRATACION,
                                bpp.NRO_CESION AS NUM_CESION,
                                bpp.CED_JURID_CESIONARIO AS NUM_CESIONARIO
                                FROM SINAEP_BOL_PAGO_PROV bpp
                                WHERE bpp.TRANSACCION = 'FP' AND (bpp.NRO_PREENTRADA_ALMACEN = ' ' AND bpp.FECHA_FACTURA_COMERCIAL >= '" + vFechaInicio + "' AND bpp.FECHA_FACTURA_COMERCIAL <= '" + vFechaFinal + "') "
                            + " AND bpp.CEDULA_PROVEEDOR = '" + vCedulaJuridica + "' "
                            + " AND bpp.NRO_FACTURA_COMERCIAL IN NVL('" + vNumFactura + "',bpp.NRO_FACTURA_COMERCIAL)"
                            + " UNION ALL "
                            + @" SELECT bpp.PROGRAMA ||'-'|| bpp.SUB_PROGRAMA AS PROGRAMA, 
                                       bpp.NRO_FACTURA_COMERCIAL AS NUM_FACTURA,
                                       bpp.DETALLE_ARTICULO AS DESC_ARTICULO,
                                       to_char(to_date(SUBSTR(bpp.FECHA_FACTURA_COMERCIAL, 5, 4) || SUBSTR(bpp.FECHA_FACTURA_COMERCIAL, 3, 2) || SUBSTR(bpp.FECHA_FACTURA_COMERCIAL, 0, 2), 'yyyymmdd'), 'dd/mm/yyyy') AS FECHA_FACTURA_COMERCIAL,
                                       CASE bpp.ESTADO_TRAMITE
                                              WHEN '01' THEN '01-REGISTRO PRE-ENTRADA'
                                              WHEN '02' THEN '02-REVISIÓN BODEGA'
                                              WHEN '03' THEN '03-VISADO TÉCNICO'
                                              WHEN '04' THEN '04-AUTORIZACION-EJECUTOR'
                                              WHEN '04' THEN '04-AUTORIZACION-EJECUTOR'
                                              WHEN '05' THEN '05-REGISTRO ENTRADA'
                                              WHEN '06' THEN '06-REGISTRO MIGO'
                                              WHEN '07' THEN '07-APLICACION BOLETA PAGO'
                                              WHEN '08' THEN '08-APLICACION ACUERDO PAGO'
                                              WHEN '09' THEN '09-ANULACIÓN BOLETA PAGO'
                                              WHEN '10' THEN '10-DESAPLICAR ACUERDO PAGO'
                                       END AS ESTADO_FACTURA,
                                       bpp.CEDULA_PROVEEDOR AS CEDULA_FACTURA,
                                       bpp.RESPALDO AS DOCUMENTO_PRESUPUESTARIO,
                                       bpp.CANTIDAD AS CANTIDAD,
                                       bpp.PRECIO AS PRECIO_FACTURA,
                                       bpp.MONTO AS MONTO_FACTURA,
                                       bpp.IMPUESTO_IVA AS MONTO_IVA,
                                       bpp.IMPUESTO_RENTA AS RENTA,
                                       bpp.MONTO_MULTA AS MULTA,
                                       --bpp.BANCO_SINPE, bpp.CUENTA_SINPE, bpp.CONTROL_SINPE,
                                       bpp.BANCO_SINPE || bpp.CUENTA_SINPE || bpp.CONTROL_SINPE AS CUENTA_DEPOSITO,
                                       DECODE(bpp.NRO_ACUERDO_PAGO, 'SN', 'SN', bpp.NRO_ACUERDO_PAGO) AS NUMERO_ACUERDO_PAGO,
                                          DECODE(NVL(bpp.FECHA_ACUERDO_PAGO, ' '), ' ', '', to_char(to_date(bpp.FECHA_ACUERDO_PAGO, 'yyyymmdd'), 'dd/mm/yyyy')) AS FECHA_ACUERDO_PAGO,
                                              (select oc.SECCIONES || '-' || sec.DESCRIPCION AS DEPENDENCIA
                                       FROM SINAEP_BOL_PAGO_PROV bpp1
                                       INNER JOIN SINAEP_ORDEN_COMPRA oc ON oc.ANO = bpp1.ANO AND oc.TRANSACCION = 'OP' AND oc.DOCUMENTO = bpp1.RESPALDO
                                       INNER JOIN sinaep_rel_estruct_prog_org sec ON sec.SECCION = oc.SECCIONES
                                       WHERE bpp1.ANO = bpp.ANO AND bpp1.TRANSACCION = bpp.TRANSACCION AND bpp1.RESPALDO = bpp.RESPALDO AND bpp1.NRO_FACTURA_COMERCIAL = bpp.NRO_FACTURA_COMERCIAL
                                       UNION
                                       select c.CENTRO_COSTO || '-' || sec.DESCRIPCION AS DEPENDENCIA
                                       FROM SINAEP_BOL_PAGO_PROV bpp2
                                       INNER JOIN SINAEP_RESERV_RECURSOS rr ON rr.ANO = bpp2.ANO AND rr.TRANSACCION = '09' AND rr.DOCUMENTO = bpp2.RESPALDO
                                       INNER JOIN ADPRF01D09_UNI_EJECUTORAS ueje ON ueje.ANO = rr.ANO AND ueje.DOCUMENTO = rr.DOCUMENTO AND ueje.TRANSACCION = '09'
                                       INNER JOIN ADPRF01D09_CENTRO_COSTO c ON c.ANO = ueje.ANO AND c.DOCUMENTO = ueje.DOCUMENTO AND c.TRANSACCION = '09' AND c.ID_U = ueje.ID_U
                                       INNER JOIN sinaep_rel_estruct_prog_org sec ON sec.SECCION = c.CENTRO_COSTO
                                       WHERE bpp2.ANO = bpp.ANO AND bpp2.TRANSACCION = bpp.TRANSACCION AND bpp2.RESPALDO = bpp.RESPALDO AND bpp2.NRO_FACTURA_COMERCIAL = bpp.NRO_FACTURA_COMERCIAL) AS DEPENDENCIA,
                                       CASE bpp.CODIGO_MONEDA
                                            WHEN '1' THEN 'COLONES'
                                            WHEN '2' THEN 'DOLARES'

                                        END AS MONEDA,
                                        bpp.TIPO_CAMBIO,
                                        bpp.NUMERO_CONTRATACION,
                                        bpp.NRO_CESION AS NUM_CESION,
                                        bpp.CED_JURID_CESIONARIO AS NUM_CESIONARIO
                                FROM SINAEP_BOL_PAGO_PROV bpp
                                WHERE
                                (bpp.TRANSACCION = 'FP' AND bpp.NRO_PREENTRADA_ALMACEN != ' '
                                AND SUBSTR(bpp.FECHA_FACTURA_COMERCIAL, 5, 4) || SUBSTR(bpp.FECHA_FACTURA_COMERCIAL, 3, 2) || SUBSTR(bpp.FECHA_FACTURA_COMERCIAL, 0, 2) >= '" + vFechaInicio + "' " 
                            + " AND SUBSTR(bpp.FECHA_FACTURA_COMERCIAL,5,4) || SUBSTR(bpp.FECHA_FACTURA_COMERCIAL,3,2)||SUBSTR(bpp.FECHA_FACTURA_COMERCIAL,0,2) <= '" + vFechaFinal + "' "
                            + " AND bpp.CEDULA_PROVEEDOR = '" + vCedulaJuridica + "') "
                            + " AND bpp.NRO_FACTURA_COMERCIAL IN NVL('" + vNumFactura + "',bpp.NRO_FACTURA_COMERCIAL)";

                DataSet dsEjecPre = DBOracle.Consultar(sql);
                List<EjecPresupuestaria> ListaFacturas = new List<EjecPresupuestaria>();

                if (dsEjecPre != null && dsEjecPre.Tables.Count > 0 && dsEjecPre.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsEjecPre.Tables[0].Rows)
                    {
                        EjecPresupuestaria factura = new EjecPresupuestaria();
                        factura.Programa = row["PROGRAMA"].ToString();
                        factura.Dependencia = row["DEPENDENCIA"].ToString();
                        factura.CedFactura = row["CEDULA_FACTURA"].ToString();
                        factura.FechaFactura = row["FECHA_FACTURA_COMERCIAL"].ToString();
                        factura.NumFactura = row["NUM_FACTURA"].ToString();
                        factura.DescArticulo = row["DESC_ARTICULO"].ToString();
                        factura.PrecioFactura = decimal.Parse(row["PRECIO_FACTURA"].ToString());
                        factura.Cantidad = int.Parse(row["CANTIDAD"].ToString());
                        factura.MontoFactura = decimal.Parse(row["MONTO_FACTURA"].ToString());
                        factura.MontoIva = decimal.Parse(row["MONTO_IVA"].ToString());
                        factura.Renta = decimal.Parse(row["RENTA"].ToString());
                        factura.Multa = decimal.Parse(row["MULTA"].ToString());
                        factura.DocPresupuestario = row["DOCUMENTO_PRESUPUESTARIO"].ToString();
                        factura.CtaDeposito = row["CUENTA_DEPOSITO"].ToString();
                        factura.EstadoFactura = row["ESTADO_FACTURA"].ToString();
                        factura.NumAcuerdoPago = row["NUMERO_ACUERDO_PAGO"].ToString();
                        factura.FechaAcuerdoPago = row["FECHA_ACUERDO_PAGO"].ToString();
                        factura.NumCesion = row["NUM_CESION"].ToString();
                        factura.NumCesionario = row["NUM_CESIONARIO"].ToString();
                        ListaFacturas.Add(factura);
                    }
                    vListaFacturas = ListaFacturas;
                }
            }
            catch (Exception ex)
            {
                Array.Resize(ref vDescMsjError, vDescMsjError.Length + 1);
                vDescMsjError[0] = msjError;
            }

        }

    }
}
