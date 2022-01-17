using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Criptografia;
using System.Data;

namespace BaseDatos
{
    public class Oracle
    {
        OracleConnection vConexion;
        OracleCommand vComando;
        OracleTransaction vTransaccion = null;
        DataTable vDataTable;
        DataSet vDataSet;
        OracleDataAdapter vDataAdapter;
        private Criptografia.Desencripta vCriptografia = new Criptografia.Desencripta();
        private string vCadenaConexion = string.Empty;

        private void Conectar()
        {
            try
            {
                // Desarrollo
                vCadenaConexion = vCriptografia.Encriptar("DATA SOURCE=vm-oracle:1521/adabas;PERSIST SECURITY INFO=True;USER ID=ADABASDEV;Password=adabasdev");
                // Produccion
                //vCadenaConexion = vCriptografia.Encriptar("DATA SOURCE=vm-oracle:1521/adabas;PERSIST SECURITY INFO=True;USER ID=ADABAS;Password=adabas");
                vConexion = new OracleConnection(vCriptografia.Desencriptar(vCadenaConexion));

                vConexion.Open();
            }
            catch (Exception ex)
            {

            }
        }

        private void Desconectar()
        {
            try
            {
                vConexion.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string ExecuteQUERY(string vSQL)
        {
            try
            {
                string pSQL = string.Empty;

                Conectar();

                vTransaccion = vConexion.BeginTransaction(IsolationLevel.ReadCommitted);

                vComando = new OracleCommand(vCriptografia.Desencriptar(vSQL), vConexion);
                vComando.CommandTimeout = 5000;
                vComando.Transaction = vTransaccion;

                //vResultado = vComando.ExecuteNonQuery();

                vTransaccion.Rollback();
                Desconectar();
                pSQL = vSQL;

                return pSQL;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExecuteCOMMIT(string[] vSQL)
        {
            try
            {
                OracleCommand[] vCMD = new OracleCommand[vSQL.Length];


                Conectar();
                vTransaccion = null;

                vTransaccion = vConexion.BeginTransaction(IsolationLevel.ReadCommitted);

                if (vSQL.Count() >= 1)
                {
                    for (int i = 0; i < vSQL.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(vSQL[i]))
                        {
                            vCMD[i] = new OracleCommand(vCriptografia.Desencriptar(vSQL[i]), vConexion);
                            string algo = vCriptografia.Desencriptar(vSQL[i]);
                            algo = vCriptografia.Desencriptar(vSQL[i]);
                            vCMD[i].Transaction = vTransaccion;
                            System.Diagnostics.Debug.WriteLine(i.ToString());
                            System.Diagnostics.Debug.WriteLine(algo);
                            vCMD[i].ExecuteNonQuery();

                        }
                    }
                }
                vTransaccion.Rollback();
                //vTransaccion.Commit();

                Desconectar();
            }
            catch (Exception ex)
            {

                vTransaccion.Rollback();
                throw ex;
            }
        }

        public DataTable LlenarDataTable(string SQL, string Base = null, bool Encriptado = false)
        {
            try
            {
                vDataTable = new DataTable();

                if (Base == null)
                    Conectar();


                if (Encriptado)
                    vDataAdapter = new OracleDataAdapter(vCriptografia.Desencriptar(SQL), vConexion);
                else
                    vDataAdapter = new OracleDataAdapter(SQL, vConexion);

                vDataAdapter.Fill(vDataTable);

                Desconectar();

                return vDataTable;
            }
            catch (Exception ex)
            {
                Desconectar();
                throw ex;
            }
        }

        public DataSet Consultar(string SQL, string Base = null, bool Encriptado = false)
        {
            try
            {
                vDataSet = new DataSet();

                if (Base == null)
                    Conectar();


                if (Encriptado)
                    vDataAdapter = new OracleDataAdapter(vCriptografia.Desencriptar(SQL), vConexion);
                else
                    vDataAdapter = new OracleDataAdapter(SQL, vConexion);

                vDataAdapter.Fill(vDataSet);

                Desconectar();

                return vDataSet;
            }
            catch (Exception ex)
            {
                Desconectar();
                throw ex;
            }
        }

    }
}
