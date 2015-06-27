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