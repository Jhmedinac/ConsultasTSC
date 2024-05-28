using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultasTSC.Models
{
    public class Upload
    {
        [Required]
        public int IdFile { get; set; }
        public string Flexfields { get; set; }
        [Required]
        public IFormFile File { get; set; }

    }

    public class FlexfieldsValues
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }

    }
}
