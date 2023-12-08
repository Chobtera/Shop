using Shop.Web.Models;

namespace Shop.Web.Services.Contracts;

public interface ICartService
{
    Task<Cart> GetCartByUserIdAsync(string userId, string token);
    Task<Cart> AddItemToCartAsync(Cart cartVM, string token);
    Task<Cart> UpdateCartAsync(Cart cartVM, string token);
    Task<bool> RemoveItemFromCartAsync(int cartId, string token);

    //implementação futura
    Task<bool> ApplyCouponAsync(Cart cartVM, string token);
    Task<bool> RemoveCouponAsync(string userId, string token);
    Task<bool> ClearCartAsync(string userId, string token);

    Task<CartHeader> CheckoutAsync(CartHeader cartHeader, string token);
}
