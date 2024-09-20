using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace RDChartSite.Core.DataAccess
{
    //Proje içerisinde Entity Framework dışında mySql vs kullanılmak istenirse fonksiyonların soyutlanması ve kullanılabilir olması için
    //arayüz olarak metodlar oluşturuldu ve repository'e arayüz eklendi,
    //orada metod gövdeleri kullanılan (EF,MySql)'e göre tanımlanabilir hale geldi.
    public interface IDataAccess<T>
    {
        
        List<T> List();

        List<T> List(Expression<Func<T, bool>> where);

        IQueryable<T> QueryableList(Expression<Func<T, bool>> where);

        int Insert(T obj);

        int Update(T obj);

        int Delete(T obj);

        int Save();


        T Find(Expression<Func<T, bool>> where);
        
    }
}
