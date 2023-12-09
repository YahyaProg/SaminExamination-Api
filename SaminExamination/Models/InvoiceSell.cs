namespace SaminExamination.Models
{
    public class InvoiceSell
    {
        public int Id { get; set; }
        public DateTime SellDate { get; set; }
        public int SellCount { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public int Count { get; set; }

    }
}
