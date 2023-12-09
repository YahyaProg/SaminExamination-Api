namespace SaminExamination.Models
{
    public class BuyInvoice
    {
        public int Id { get; set; }
        public DateTime RegisterDate { get; set; }
        public int Count { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public bool updatePrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
