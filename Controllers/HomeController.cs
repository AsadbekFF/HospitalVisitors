using HospitalVisitors.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HospitalVisitors.Controllers
{
    public class HomeController : Controller
    {
        public static List<Visitors> visitorsData = new();

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Index(Visitors visitors)
        {
            SqlConnection conn = new(@"Server=(localdb)\MSSQLLocalDB;Database=Hospital;Trusted_Connection=True;");
            //SqlConnection conn = new(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Hospital;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            conn.Open();
            //CheckingFormatOfVisitorInput(visitors);
            SqlCommand cmd = new($"INSERT INTO HospitalTable (UserName, UserDate, UserLogs, UserType) VALUES (N'{visitors.Name}', GETDATE(), N'{visitors.Logs}', N'{visitors.Type}')", conn);
            
            cmd.ExecuteNonQuery();

            ViewData["Visitors"] = await GetVisitorsAsync();

            return View();
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Visitors"] = await GetVisitorsAsync();

            return View();
        }

        public async Task<List<Visitors>> GetVisitorsAsync()
        {
            List<Visitors> visitors = new List<Visitors>();
            SqlConnection conn = new(@"Server=(localdb)\MSSQLLocalDB;Database=Hospital;Trusted_Connection=True;");
            //SqlConnection conn = new(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Hospital;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            conn.Open();

            SqlCommand sqlCommandGetData = new("Select * from HospitalTable", conn);

            using (SqlDataReader reader = sqlCommandGetData.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    Visitors visitor = new();
                    visitor.ID = (int)reader["ID"];
                    visitor.Name = reader["UserName"].ToString();
                    visitor.Date = reader["UserDate"].ToString();
                    visitor.Logs = reader["UserLogs"].ToString();
                    visitor.Type = reader["UserType"].ToString();

                    visitors.Add(visitor);
                }
            }

            return visitors;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
