using Microsoft.EntityFrameworkCore;
using System;

namespace HospitalVisitors.Models
{
    public class Visitors
    {
        public int ID { get; set; }
        public string Name { get; set; }

        //public string Name { get; set; }
        public string Logs { get; set; }
        public string Type { get; set; }

        public string Date { get; set; } = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
    }
}
