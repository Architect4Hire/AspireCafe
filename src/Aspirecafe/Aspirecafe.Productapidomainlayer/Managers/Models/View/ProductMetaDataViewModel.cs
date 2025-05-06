using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.ProductApiDomainLayer.Managers.Models.View
{
    public class ProductMetaDataViewModel
    {
        public List<Guid> ProductIds { get; set; } = new List<Guid>();
    }
}
