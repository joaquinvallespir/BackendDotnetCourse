using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Sale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SaleId { get; set; }
        public string ClientName { get; set; }
        public int BeerId { get; set; }
        public int BrandId { get; set; }
        public string BeerName { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        
        public DateTime BuyDatetime { get; set; }
        [ForeignKey("BeerId")]
        public virtual Beer Beer { get; set; }

        

        
    }
}
