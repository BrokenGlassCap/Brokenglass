using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrokenGlassWebApp.Models
{
    public class ClaimGridViewModel
    {
        public int Id { get; set; }
        public DateTime DateCreate { get; set; }
        public string StateName { get; set; }
        public string ServiceName { get; set; }
        public string AdressName { get; set; }
        public int OsbCode { get; set; }
        public string UserEmail { get; set; }

    }
}