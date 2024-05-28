using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultasTSC.Models
{
    public class UploadServiceResponse
    {
        public int ReferenceId { get; set; }
        public string directory { get; set; }

        public string error { get; set; }
    }
}
