using System.ComponentModel.DataAnnotations;

namespace Inventory.Domains
{
    public abstract class Person : BaseTable 
    {

        [Required, MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(50)]
        public string? Phone { get; set; }

        [MaxLength(150)]
        public string? Email { get; set; }

        public string? Address { get; set; }
    }





}
