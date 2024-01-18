using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Person
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public DateTime birth_date { get; set; }
    }
}
