using System.ComponentModel.DataAnnotations;

namespace Inventory.Domains
{
    public class StockTransaction : BaseTable
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int ChangeQty { get; set; }

        [Required, MaxLength(50)]
        public string Type { get; set; } = null!; // purchase, sale, adjust, return

        public int? ReferenceId { get; set; }

    }





}
