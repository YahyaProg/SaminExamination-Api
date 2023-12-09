namespace SaminExamination.Dto
{
    public class BuyInvoiceDto
    {
        public DateTime RegisterDate { get; set; }
        public int Count { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public bool UpdatePrice { get; set; }
    }
}
