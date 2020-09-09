using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Q3Books.Models
{
    public class UserModel
    {
        public int ID { get; set; }
        public string team_code { get; set; }
        public string job_number { get; set; }
        public string name { get; set; }
        public string organization { get; set; }
        public string floor { get; set; }
        public string character { get; set; }
    }
}
