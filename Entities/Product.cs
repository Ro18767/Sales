using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp12.Entities
{
    internal class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public double Price { get; set; }

        public string GetShortString()
        {
            return $"{this.Id.ToString()[..4] + ".."} {this.Name} {this.Price}";
        }
    }
}
