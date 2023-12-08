using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Shop.Web.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public int Stock { get; set; }
        //public string CategoryName { get; set; }

        public Category Category { get; set; }
        public int CategoryId { get; set; }

    }
}
