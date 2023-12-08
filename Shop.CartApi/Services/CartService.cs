using Microsoft.EntityFrameworkCore;
using Shop.CartApi.Context;
using Shop.CartApi.Models;

namespace Shop.CartApi.Services;

public class CartService : ICartService
{
    private readonly AppDbContext _context;

    public CartService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CleanCartAsync(string userId)
    {
        var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);

        if (cartHeader is not null)
        {
            _context.CartItems.RemoveRange(_context.CartItems.Where(c => c.CartHeaderId == cartHeader.Id));
            _context.CartHeaders.Remove(cartHeader);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<Cart> GetCartByUserIdAsync(string userId)
    {
        Cart cart = new()
        {
            CartHeader = await _context.CartHeaders
                               .FirstOrDefaultAsync(c => c.UserId == userId),
        };

        cart.CartItems = _context.CartItems
                        .Where(c => c.CartHeaderId == cart.CartHeader.Id)
                        .Include(c => c.Product);

        return cart;
    }

    public async Task<bool> DeleteItemCartAsync(int cartItemId)
    {
        try
        {
            CartItem cartItem = await _context.CartItems
                               .FirstOrDefaultAsync(c => c.Id == cartItemId);

            int total = _context.CartItems.Where(c => c.CartHeaderId == cartItem.CartHeaderId).Count();

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            if (total == 1)
            {
                var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(
                                             c => c.Id == cartItem.CartHeaderId);

                _context.CartHeaders.Remove(cartHeader);
                await _context.SaveChangesAsync();
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<Cart> UpdateCartAsync(Cart Cart)
    {

        //salva o produto no banco 
        await SaveProductInDataBase(Cart);

        //Verifica se o CartHeader é null
        var cartHeader = await _context.CartHeaders.AsNoTracking().FirstOrDefaultAsync(
                               c => c.UserId == Cart.CartHeader.UserId);

        if (cartHeader is null)
        {
            //criar o Header e os itens
            await CreateCartHeaderAndItems(Cart);
        }
        else
        {
            //atualiza a quantidade e os itens
            await UpdateQuantityAndItems(Cart, cartHeader);
        }
        return Cart;
    }

    private async Task UpdateQuantityAndItems(Cart cart, CartHeader cartHeader)
    {
        //Se CartHeader não é null
        //verifica se CartItems possui o mesmo produto
        var cartItem = await _context.CartItems.AsNoTracking().FirstOrDefaultAsync(
                               p => p.ProductId == cart.CartItems.FirstOrDefault()
                               .ProductId && p.CartHeaderId == cartHeader.Id);

        if (cartItem is null)
        {
            //Cria o CartItems
            cart.CartItems.FirstOrDefault().CartHeaderId = cartHeader.Id;
            cart.CartItems.FirstOrDefault().Product = null;
            _context.CartItems.Add(cart.CartItems.FirstOrDefault());
            await _context.SaveChangesAsync();
        }
        else
        {
            //Atualiza a quantidade e o CartItems
            cart.CartItems.FirstOrDefault().Product = null;
            cart.CartItems.FirstOrDefault().Quantity += cartItem.Quantity;
            cart.CartItems.FirstOrDefault().Id = cartItem.Id;
            cart.CartItems.FirstOrDefault().CartHeaderId = cartItem.CartHeaderId;
            _context.CartItems.Update(cart.CartItems.FirstOrDefault());
            await _context.SaveChangesAsync();
        }
    }

    private async Task CreateCartHeaderAndItems(Cart cart)
    {
        //Cria o CartHeader e o CartItems
        _context.CartHeaders.Add(cart.CartHeader);
        await _context.SaveChangesAsync();

        cart.CartItems.FirstOrDefault().CartHeaderId = cart.CartHeader.Id;
        cart.CartItems.FirstOrDefault().Product = null;

        _context.CartItems.Add(cart.CartItems.FirstOrDefault());

        await _context.SaveChangesAsync();
    }

    private async Task SaveProductInDataBase(Cart Cart)
    {
        //Verifica se o produto ja foi salvo senão salva
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id ==
                            Cart.CartItems.FirstOrDefault().ProductId);

        //salva o produto senão existe no bd
        if (product is null)
        {
            var teste = Cart.CartItems.FirstOrDefault().Product;
            //teste.Id = 0;
            _context.Products.Add(teste);
            //_context.Entry(teste).State = EntityState.Added;
            await _context.SaveChangesAsync();
            Console.Write("TesteLinha");
        }
        //if (Cart.CartItems. == 0)
        //{
        //    context.Entry(product).State = EntityState.Added;
        //}
        //else
        //{
        //    context.Entry(product).State = EntityState.Modified;
        //}
        //context.SaveChanges();
    }

    public async Task<bool> ApplyCouponAsync(string userId, string couponCode)
    {
        var cartHeaderApplyCoupon = await _context.CartHeaders
                               .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cartHeaderApplyCoupon is not null)
        {
            cartHeaderApplyCoupon.CouponCode = couponCode;

            _context.CartHeaders.Update(cartHeaderApplyCoupon);

            await _context.SaveChangesAsync();

            return true;
        }
        return false;
    }

    public async Task<bool> DeleteCouponAsync(string userId)
    {
        var cartHeaderDeleteCoupon = await _context.CartHeaders
                              .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cartHeaderDeleteCoupon is not null)
        {
            cartHeaderDeleteCoupon.CouponCode = "";

            _context.CartHeaders.Update(cartHeaderDeleteCoupon);

            await _context.SaveChangesAsync();

            return true;
        }
        return false;
    }
}