using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApp.Models {
    public class Supplier {

        public long SupplierId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
