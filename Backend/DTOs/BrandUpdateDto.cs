using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.DTOs
{
    public class BrandUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
