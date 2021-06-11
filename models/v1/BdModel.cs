using System;
using System.Data;
using System.IO;
using Advantage.Data.Provider;

namespace TicketingApi.Models.v1
{
    public class BdModel
    {
        public AdsConnection conn;
        public DataTable GetData(string path)
        {
            DataTable dt = new DataTable();
           // var targetLocation = Path.Combine(path, "scriptbbb.sql");
            string sqlText = "SELECT TOP 200 * FROM ORT2_20"; // File.ReadAllText(@targetLocation);
            using (AdsCommand cmd = new AdsCommand(sqlText, conn))
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
            return dt;
        }
    }
}