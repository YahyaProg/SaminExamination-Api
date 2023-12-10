using SaminExamination.Models;

namespace SaminExamination.Dto
{
    public class ProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int count { get; set; }
        public DateTime EXP { get; set; }
        public DateTime RegisterDate { get; set; }
    }

    public class GetProductDto
    {
        public int PageSiz { get; set; }
        public int PageNumber { get; set; }
    }
    public class ProductUpdateDto
    {
       public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public int count { get; set; }
        public DateTime EXP { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
