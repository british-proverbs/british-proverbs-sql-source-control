using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BritishProverbs.Web.Models;

namespace BritishProverbs.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            RecordVisit(Request.UserHostAddress);
            var proverbModel = GetRandomProverb();

            return View(proverbModel);
        }

        private ProverbModel GetRandomProverb()
        {
            const string selectStatement = "EXEC dbo.SelectRandomProverb";
            var connectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = selectStatement;
                    cmd.CommandType = CommandType.Text;

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        return reader.Select(r => ProverbModelBuilder(r)).First();
                    }
                }
            }
        }

        private void RecordVisit(string ipAddress)
        {
            const string insertStatement = "INSERT INTO Visits(IpAddress, CreatedOn) VALUES(@IpAddress, @CreatedOn)";
            var connectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand(insertStatement, conn))
                {
                    cmd.Parameters.Add("IpAddress", SqlDbType.NVarChar).Value = ipAddress;
                    cmd.Parameters.Add("CreatedOn", SqlDbType.DateTimeOffset).Value = DateTimeOffset.Now;

                    conn.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private ProverbModel ProverbModelBuilder(SqlDataReader reader)
        {
            return new ProverbModel
            {
                Id = int.Parse(reader["Id"].ToString()),
                Content = reader["Content"] is DBNull ? null : reader["Content"].ToString()
            };
        }
    }

    public static class Extensions
    {

        public static IEnumerable<T> Select<T>(
            this SqlDataReader reader, Func<SqlDataReader, T> projection)
        {

            while (reader.Read())
            {
                yield return projection(reader);
            }
        }
    }
}