using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using WorkerFuncionariosIntranet.Services;

namespace WorkerFuncionariosIntranet
{
    public partial class Service1 : ServiceBase
    {
        int intervalo = Convert.ToInt32(ConfigurationManager.AppSettings["timer"].ToString());

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Thread thread = new Thread(Start);
            //Start(null);
            thread.Start();
        }

        protected override void OnStop()
        {
        }

        protected void Start(object sender)
        {
            while (true)
            {

                string _procedure = "DBO.SP_COR_SEL_FuncionariosIntranet";
                string _caminhotxt = ConfigurationManager.AppSettings["path"].ToString();

                //1 Chamar a Procedure
                var myDataSet = new DataSet();
                myDataSet = SelectRowsProc(myDataSet, _procedure);

                string _txtFile = "";
                foreach (DataRow table in myDataSet.Tables[0].Rows)
                {
                    _txtFile = _txtFile + table[0].ToString();
                }
                File.WriteAllText(_caminhotxt, _txtFile);
                Thread.Sleep(intervalo);
            }
        }

        private DataSet SelectRowsProc(DataSet dataSet, string proc)
        {
            using (var conn = SingleConnection.GetConnection())
            {

                using (var adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = new SqlCommand(proc, (SqlConnection)conn);
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    adapter.SelectCommand.CommandTimeout = 180000;
                    adapter.Fill(dataSet);
                    SingleConnection.CloseConnection();
                    return dataSet;
                }
            };
        }
    }
}
