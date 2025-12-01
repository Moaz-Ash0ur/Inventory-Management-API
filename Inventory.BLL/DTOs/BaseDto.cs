namespace Inventory.BLL.DTOs
{
    public class BaseDto
    {
        public int Id { get; set; }
    }

    public class SaleDto : BaseDto
    {
        public int? CustomerId { get; set; }
        public string? InvoiceNo { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public List<SaleItemDto> SaleItems { get; set; } = new List<SaleItemDto>();
    }

    public class SaleItemDto : BaseDto
    {
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal CostAtSale { get; set; }//to know the price when you buy from supplier
                                               //how much when sle it to customer to knw the profit
    }

    public class StockTransactionDto : BaseDto
    {
        public int ProductId { get; set; }
        public int ChangeQty { get; set; }
        public string Type { get; set; } = null!;
        public int? ReferenceId { get; set; }
    }
}
