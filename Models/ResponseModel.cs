using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Q3Books.Models
{
    public class ResponseModel
    {
        public int status { get; set; }
        public string statusText { get; set; }
        public object data { get; set; }
    }
}
