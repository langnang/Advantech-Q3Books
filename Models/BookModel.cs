using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Q3Books.Models
{
    public class BookModel
    {
        public int ID { get; set; }
        public string user { get; set; }
        public string book { get; set; }
        public DateTime datetime { get; set; }
        public Boolean custom { get; set; }

        public string annotation { get; set; }
    }

    public class BookItemModel
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string author { get; set; }
        public string publisher { get; set; }
        public Boolean ddzy { get; set; }
        public float price { get; set; }
    }
}
