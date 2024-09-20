using RDChartSite.Core.DataAccess;
using RDChartSite.DataAccessLayer;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RDChartSite.DataAccessLayer.EntitiyFramework
{
    public class Repository<T> :RepositoryBase, IDataAccess<T> where T : class  
    {   
        
        private DbSet<T> _dbset;
        public Repository()
        {
            
            _dbset = context.Set<T>();
        }
        public List<T> List()
        {
            return _dbset.ToList();   
        }
        public List<T> List(Expression<Func<T,bool>> where)
        {
            return _dbset.Where(where).ToList();    
        }
        public IQueryable<T> QueryableList(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where);
        }
        public int Insert(T obj) 
        {
            _dbset.Add(obj);
            return Save();
        }
        public int Update(T obj)
        {
            _dbset.AddOrUpdate(obj); 
            return Save();
        }
        public int Delete(T obj)
        {
            _dbset.Remove(obj);
            return Save();
        }
        public int Save()
        {
            return context.SaveChanges();
        }

        public T Find(Expression<Func<T, bool>> where) 
        {
            return _dbset.FirstOrDefault(where);
        }
    }
}
