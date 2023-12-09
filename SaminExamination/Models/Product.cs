namespace SaminExamination.Models
{
    public class Product
    {
       public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int count { get; set; }
        public DateTime EXP {get; set; }
        public DateTime RegisterDate { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }

    }
}
