using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Students
    {
        public Students()
        {

        }

        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Major { get; set; }
        public decimal Average { get; set; }
        public string UserId { get; set; }


        public int ClassName { get; set; }

        [ForeignKey("ClassName")]
        public StdClass Class { get; set; }
    }
}