using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultasTSC.Models
{
    public class GetFileDataResponse
    {
        public string FileData { get; set; }
        public string FileUrl { get; set; }
        public string Error { get; set; }
    }
}
