using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerFuncionariosIntranet.Services
{
    class SingleConnection
    {

        private static SqlConnection Con = null;

        public static SqlConnection GetConnection()
        {
            try
            {
                string ConnectionString = ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                if (Con != null)
                {
                    if (Con.State != ConnectionState.Open)
                    {
                        Con = new SqlConnection(ConnectionString);
                        Con.Open();

                        return Con;
                    }

                    else return Con;
                }

                Con = new SqlConnection(ConnectionString);
                Con.Open();
                return Con;
            }
            catch (Exception e)
            {
                EventLog InsereLog = new EventLog();
                InsereLog.Source = "GetConnection Woker";
                InsereLog.WriteEntry(e.Message);
                InsereLog.Dispose();

                return Con;
            }
        }

        public static void CloseConnection()
        {
            try
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }

            }
            catch (Exception e)
            {
                EventLog InsereLog = new EventLog();
                InsereLog.Source = "CloseConnection Woker";
                InsereLog.WriteEntry(e.Message);
                InsereLog.Dispose();
            }
        }
    }

}

