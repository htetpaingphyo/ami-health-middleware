using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace Ami.Health.Middleware.DataModels
{
    public class OracleDbContext
    {
        //private string CONN_STRING = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=172.20.122.110)(PORT=1525))(CONNECT_DATA=(SERVICE_NAME=SICL))); User Id=SICL; Password=GoGoLive785";
        private string CONN_STRING = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=172.20.189.110)(PORT=1525))(CONNECT_DATA=(SERVICE_NAME=SICL))); User Id=SICL; Password=GoGoLive785";

        private OracleConnection connection = null;

        public OracleDbContext()
        {
            connection = new OracleConnection(CONN_STRING);
            CheckConnectionState();
        }

        public DataTable GetPopulatedData(string policy)
        {
            try
            {
                DataTable TempData = new DataTable();

                ClearTrans();
                PopulateData(policy);

                OracleCommand OraCommand2 = connection.CreateCommand();
                OraCommand2.CommandType = CommandType.Text;
                OraCommand2.CommandText = "SELECT * FROM VW_LIFE_DATA";

                CheckConnectionState();
                OracleDataReader reader = OraCommand2.ExecuteReader();

                if (reader.HasRows)
                {
                    TempData.Load(reader);
                }
                else
                {
                    throw new Exception("No data found!");
                }

                return TempData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public DataTable GetRenewalData(DateTime from, DateTime to)
        {
            try
            {
                DataTable TempData = new DataTable();

                ClearTrans();
                PopulateRenewalData(from, to);

                OracleCommand OraCommand2 = connection.CreateCommand();
                OraCommand2.CommandType = CommandType.Text;
                OraCommand2.CommandText = "SELECT * FROM VW_HEALTH_RENEWAL";

                CheckConnectionState();
                OracleDataReader reader = OraCommand2.ExecuteReader();

                if (reader.HasRows)
                {
                    TempData.Load(reader);
                }
                else
                {
                    throw new Exception("No data found!");
                }

                return TempData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        private void PopulateRenewalData(DateTime from, DateTime to)
        {
            try
            {
                OracleCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PK_STAT_REPORTS.PR_POP_X_DATA_LIFE_REN_NOTICE";
                command.Parameters.Add("wkfromdt", from).Direction = ParameterDirection.Input;
                command.Parameters.Add("wktodt", to).Direction = ParameterDirection.Input;

                CheckConnectionState();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            //EXEC PK_STAT_REPORTS.PR_POP_X_DATA_LIFE_REN_NOTICE('01-AUG-2018', '31-AUG-2018');
        }

        private void PopulateData(string policy)
        {
            try
            {
                OracleCommand OraCommand = connection.CreateCommand();
                OraCommand.CommandType = CommandType.StoredProcedure;
                OraCommand.CommandText = "PK_STAT_REPORTS.PR_POP_X_DATA_POLICY";
                OraCommand.Parameters.Add("wkpolicyno", policy).Direction = ParameterDirection.Input;

                CheckConnectionState();
                OraCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        private void ClearTrans()
        {
            try
            {
                OracleCommand OraCommand = connection.CreateCommand();
                OraCommand.CommandType = CommandType.StoredProcedure;
                OraCommand.CommandText = "PK_STAT_REPORTS.PR_DEL_X_DATA";

                CheckConnectionState();
                OraCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        private void CheckConnectionState()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}