using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tasklistcs
{
    class Element
    {
        public int id;
        public string name;
        public string place;
        public DateTime date;
        public Element(int id, string name, string place, DateTime date)
        {
            this.id = id;
            this.name = name;
            this.place = place;
            this.date = date;
        }
    }
}
