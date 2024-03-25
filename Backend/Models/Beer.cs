using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Beer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BeerId { get; set; }

        public string Name { get; set; }
        public int StockQuantity {  get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
       
        public int BrandId {  get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Alcohol {  get; set; }
        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }
    }
}
