using RDChartSite.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDChartSite.DataAccessLayer.EntitiyFramework
{
    public class RepositoryBase
    {
        protected static RdDbContext context;
        private static object _LockObj = new object(); 
        protected RepositoryBase() 
        {
           CreateContext();
        }
        public static void CreateContext() 
        {
            if (context == null)
            {
                lock (_LockObj)
                {
                    context = new RdDbContext();
                }
            }
           

        }
    }
}
