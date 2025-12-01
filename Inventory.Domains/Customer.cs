namespace Inventory.Domains
{
    public class Customer : Person
    {

        // Navigation
        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }





}
