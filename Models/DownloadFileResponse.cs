using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultasTSC.Models
{
    public class DownloadFileResponse<T>
    {
        public string Data { get; set; }
        public T DownloadableFile { get; set; }
    }
}
