using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseRequest__.Net_Framework_.Models
{
    public class Request
    {
        [Key]
        public int Id { get; set; }
        public string Full_Name { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Course_Name { get; set; }
        public string Course_Type { get; set; }
        public string Notation { get; set; }
        public string Status { get; set; }
        public DateTime Course_Start { get; set; }
        public DateTime Course_End { get; set; }
        public int Year { get; set; }
        public string User { get; set; }
    }
}