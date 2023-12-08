using Shop.CartApi.Models;

namespace Shop.CartApi.Services;

public interface ICartService
{
    Task<Cart> GetCartByUserIdAsync(string userId);
    Task<Cart> UpdateCartAsync(Cart cart);
    Task<bool> CleanCartAsync(string userId);
    Task<bool> DeleteItemCartAsync(int cartItemId);

    Task<bool> ApplyCouponAsync(string userId, string couponCode);
    Task<bool> DeleteCouponAsync(string userId);
}
