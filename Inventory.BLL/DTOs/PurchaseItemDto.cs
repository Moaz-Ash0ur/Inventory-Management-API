using Inventory.Domains;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.BLL.DTOs
{
    public class PurchaseItemDto : BaseDto
    {       
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal? TotalCost { get; set; }
    }
   
}
