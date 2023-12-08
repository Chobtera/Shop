namespace Shop.Web.Models;

public class Cart
{
    public CartHeader CartHeader { get; set; } = new CartHeader();
    public IEnumerable<CartItem> CartItems { get; set; }
}
