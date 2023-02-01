using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp12.Entities
{
    public class Manager
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Secname { get; set; } = null!;

        

        public string GetShortString()
        {
            return $"{this.Id.ToString()[..4] + ".."} {this.Name} {this.Surname}";
        }
    }
}
