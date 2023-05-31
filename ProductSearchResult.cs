using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRF.sample
{
    public class ProductSearchResult : SearchResult
    {
        public IEnumerable<Product>? Products { get; set; }
    }
}
