using authentication.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Runtime.Intrinsics.X86;

namespace authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase

    {
        private JwtContext _context;

        public UserController(JwtContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserTb>>> GetUser()
        {
            return Ok(await _context.UserTbs.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserTb user1)

        {
            _context.UserTbs.Add(user1);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetUser", new { id = user1.Id }, user1);
        }


    }
}
