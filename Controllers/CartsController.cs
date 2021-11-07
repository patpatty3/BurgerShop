using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BurgerShop.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers {
    [Route("api/[controller]")]
    public class CartsController: ControllerBase {
        private DataContext context;

        public CartsController(DataContext ctx) {
            context = ctx;
        }

        [HttpGet]
        public IAsyncEnumerable<Cart> GetCarts() {
            return context.Carts;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCart(long id) {
            Cart c = await context.Carts
                .Include(c => c.CartLines)
                .ThenInclude(cl => cl.Menu)
                .FirstAsync(c => c.CartId == id);
            if (c == null) {
                return NotFound();
            }
            return Ok(c);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart([FromBody]CartBindingTarget target) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            Cart c = target.ToCart();
            await context.Carts.AddAsync(c);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCart), new {id = c.CartId}, c);
        }

        [HttpPut]
        public async Task UpdateCart([FromBody]Cart cart) {
            context.Carts.Update(cart);
            await context.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public async Task DeleteCart(long id) {
            context.Carts.Remove(new Cart() {CartId = id});
            await context.SaveChangesAsync();
        }
    }
}
