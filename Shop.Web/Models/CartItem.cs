﻿namespace Shop.Web.Models;

public class CartItem
{
    public int Id { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; } = 1;
    public int ProductId { get; set; }
    public int CartHeaderId { get; set; }
}
