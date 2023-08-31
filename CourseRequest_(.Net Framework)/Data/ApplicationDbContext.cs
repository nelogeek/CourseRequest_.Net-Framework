using CourseRequest__.Net_Framework_.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CourseRequest__.Net_Framework_.Data
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Request> Requests { get; set; }
    }
}