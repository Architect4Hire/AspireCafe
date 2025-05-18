using AspireCafe.Shared.Enums;
using AspireCafe.Shared.Models.Message.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.Shared.Models.Message.Barista
{
    public class BaristaOrderMessageModel : MessageBaseModel
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; }
        public int TableNumber { get; set; }
        public List<ProductInfoMessageModel> Items { get; set; }
        public RouteType RouteType { get; set; }

        public BaristaOrderMessageModel()
        {
            Items = new List<ProductInfoMessageModel>();
        }
    }
}
