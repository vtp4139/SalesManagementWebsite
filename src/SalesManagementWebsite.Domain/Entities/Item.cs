﻿
namespace SalesManagementWebsite.Domain.Entities
{
    public class Item : BaseModel
    {
        public string Name { get; set; } = string.Empty;   
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        //Foreign key for Standard
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        //Foreign key for Standard
        public Guid BrandId { get; set; }
        public Brand Brand { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
