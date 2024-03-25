namespace Backend.DTOs
{
    public class SaleDto
    {
        public long Id { get; set; }
        public string ClientName { get; set; }
        public int BeerId { get; set; }
        public int BrandId { get; set; }
        public string BeerName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

        public DateTime BuyDatetime { get; set; }
    }
}
