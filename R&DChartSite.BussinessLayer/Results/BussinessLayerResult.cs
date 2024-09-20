using RDChartSite.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDChartSite.BussinessLayer.Results
{
    public class BussinessLayerResult<T> where T : class
    {
        public T Results { get; set; }
        public List<ErrorMessageObj> Errors { get; set; }
        public BussinessLayerResult()
        {
            Errors = new List<ErrorMessageObj>();
            
        }
        public void AddError(ErrorMessageCode code, string message)
        {
            Errors.Add(new ErrorMessageObj() { Code = code,Message =  message });  
        }

    }
}
