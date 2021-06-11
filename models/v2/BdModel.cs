using System;
using System.Data;
using System.IO;
using Advantage.Data.Provider;

namespace TicketingApi.Models.v2
{
    public class BdModel
    {
         private AdsConnection GetConnection()
        {
            AdsConnection conn = new AdsConnection("data source =\\\\SQL2009\\DATADOS\\EPS\\DATA\\ADS\\DAN-GROUP\\DENTSU\\DIAMGI\\DIAMGI.ADD; user id = AdsSys; password = u$3R4dm1n; ServerType = REMOTE; LockMode = COMPATIBLE; ShowDeleted = False; ");
            return conn;
        }

        public DataTable GetData(string path)
        {
            DataTable dt = new DataTable();
           // var targetLocation = Path.Combine(path, "scriptbbb.sql");
            string sqlText = "SELECT TOP 200 * FROM ORT2_20"; // File.ReadAllText(@targetLocation);
            AdsConnection Conn = GetConnection();
            Conn.Open();
            using (AdsCommand cmd = new AdsCommand(sqlText, Conn))
            {
                // cmd.Parameters.Add("D_BEGIN", "2020-01-01");
                // cmd.Parameters.Add("D_END", "2020-01-31");
                // cmd.Parameters.Add("D_YEAR", "2020-01-01");
                // cmd.Parameters.Add("BY_VERSION", "0");
                // cmd.Parameters.Add("AGENCYNO", "DIAMGI");
                using (AdsDataReader dr = cmd.ExecuteReader()) {
                    dt.Load(dr);
                }
            }
            Conn.Close();
            return dt;
        }
    }
}