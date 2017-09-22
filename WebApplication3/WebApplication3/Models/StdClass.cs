using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class StdClass
    {
        public StdClass()
        {

        }
        [Key]
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string UserId { get; set; }
        public ICollection<Students> Students { get; set; }
    }
}