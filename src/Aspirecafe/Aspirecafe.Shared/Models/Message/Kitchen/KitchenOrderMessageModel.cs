using AspireCafe.Shared.Models.Message.Shared;

namespace AspireCafe.Shared.Models.Message.Kitchen
{
    public class KitchenOrderMessageModel:MessageBaseModel
    {
        public int CustomerName { get; set; }
        public int TableName { get; set; }
        public List<ProductInfoMessageModel> Items { get; set; }

        public KitchenOrderMessageModel()
        {
            Items = new List<ProductInfoMessageModel>();
        }
    }
}
