using RDChartSite.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RDChartSite.DataAccessLayer.EntitiyFramework
{
    public class RdDbContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Projects> Projects { get; set; }   
        public DbSet<DailyActivity> DailyActivities { get; set; }
        public DbSet<HourlyActivity> HourlyActivity { get; set; }

        public DbSet<OffDayForms> OffDayForms { get; set; }

        public DbSet<Teams> Teams { get; set; }


    }
}
