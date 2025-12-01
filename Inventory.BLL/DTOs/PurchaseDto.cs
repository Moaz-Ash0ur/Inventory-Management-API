namespace Inventory.BLL.DTOs
{
    public class PurchaseDto : BaseDto
    {
        public int? SupplierId { get; set; }
        public string? InvoiceNo { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalAmount { get; set; } 
        public List<PurchaseItemDto> Items { get; set; } = new List<PurchaseItemDto>();
    }

}
