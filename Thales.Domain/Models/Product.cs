namespace Thales.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public IEnumerable<string> Images { get; set; }
        public string CreationAt { get; set; }
        public string UpdatedAt { get; set; }
        public decimal Tax { get; set; }
    }
}
