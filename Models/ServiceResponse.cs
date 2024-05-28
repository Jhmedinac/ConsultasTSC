using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultasTSC.Models
{
    public class ServiceResponse<T>
    {
        public T Result { set; get; }
        public bool Success { set; get; }
        public string Message { set; get; }
    }
}
