using Microsoft.AspNetCore.Mvc;
using Shop.CartApi.Models;
using Shop.CartApi.Models;
using Shop.CartApi.RabbitMQSender;
using Shop.CartApi.Services;
namespace Shop.CartApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartService _Service;
    private readonly IRabbitMQMessageSender _rabbitMQMessageSender;

    public CartController(ICartService service, IRabbitMQMessageSender rabbitMQMessageSender)
    {
        _Service = service;
        _rabbitMQMessageSender = rabbitMQMessageSender;
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<CheckoutHeader>> Checkout(CheckoutHeader checkoutDto)
    {
        var cart = await _Service.GetCartByUserIdAsync(checkoutDto.UserId);

        if (cart is null)
        {
            return NotFound($"Cart Not found for {checkoutDto.UserId}");
        }

        checkoutDto.CartItems = cart.CartItems;
        checkoutDto.DateTime = DateTime.Now;

        _rabbitMQMessageSender.SendMessage(checkoutDto, "checkoutqueue");

        return Ok(checkoutDto);
    }

    [HttpPost("applycoupon")]
    public async Task<ActionResult<Cart>> ApplyCoupon(Cart Cart)
    {
        var result = await _Service.ApplyCouponAsync(Cart.CartHeader.UserId,
                                                        Cart.CartHeader.CouponCode);

        if (!result)
        {
            return NotFound($"CartHeader not found for userId = {Cart.CartHeader.UserId}");
        }
        return Ok(result);
    }

    [HttpDelete("deletecoupon/{userId}")]
    public async Task<ActionResult<Cart>> DeleteCoupon(string userId)
    {
        var result = await _Service.DeleteCouponAsync(userId);

        if (!result)
        {
            return NotFound($"Discount Coupon not found for userId = {userId}");
        }

        return Ok(result);
    }

    [HttpGet("getcart/{userid}")]
    public async Task<ActionResult<Cart>> GetByUserId(string userid)
    {
        var Cartj = await _Service.GetCartByUserIdAsync(userid);

        if (Cartj is null)
            return NotFound();

        return Ok(Cartj);
    }


    [HttpPost("addcart")]
    public async Task<ActionResult<Cart>> AddCart(Cart Cart)
    {
        var cart = await _Service.UpdateCartAsync(Cart);

        if (cart is null)
            return NotFound();

        return Ok(cart);
    }

    [HttpPut("updatecart")]
    public async Task<ActionResult<Cart>> UpdateCart(Cart Cart)
    {
        var cart = await _Service.UpdateCartAsync(Cart);
        if (cart == null) return NotFound();
        return Ok(cart);
    }

    [HttpDelete("deletecart/{id}")]
    public async Task<ActionResult<bool>> DeleteCart(int id)
    {
        var status = await _Service.DeleteItemCartAsync(id);

        if (!status)
            return BadRequest();

        return Ok(status);
    }
}
