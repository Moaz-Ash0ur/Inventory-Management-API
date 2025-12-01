namespace Inventory.Domains
{
    public class Supplier : Person
    {      
        // Navigation
        public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
    }





}
