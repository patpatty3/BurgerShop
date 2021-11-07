using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BurgerShop.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers {
    [Route("api/[controller]")]
    public class MenusController: ControllerBase {
        private DataContext context;

        public MenusController(DataContext ctx) {
            context = ctx;
        }

        [HttpGet]
        public IAsyncEnumerable<Menu> GetMenus() {
            return context.Menus;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenu(long id) {
            Menu m = await context.Menus.FindAsync(id);
            if (m == null) {
                return NotFound();
            }
            return Ok(m);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateMenu([FromBody]MenuBindingTarget target) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            Menu m = target.ToMenu();
            await context.Menus.AddAsync(m);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMenu), new {id = m.MenuId}, m);
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task UpdateMenu([FromBody]Menu menu) {
            context.Menus.Update(menu);
            await context.SaveChangesAsync();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task DeleteMenu(long id) {
            context.Menus.Remove(new Menu() {MenuId = id});
            await context.SaveChangesAsync();
        }
    }
}
