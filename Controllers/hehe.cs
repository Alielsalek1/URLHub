using ALL.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using URLshortner.Models;

namespace URLshortner.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Hehe : ControllerBase
    {
        private readonly AppDbContext _Dbcontext;
        public Hehe(AppDbContext dbcontext) { _Dbcontext = dbcontext; }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<int>> AddUser(User user)
        {
            user.ID = 0;
            _Dbcontext.Set<User>().Add(user);
            await _Dbcontext.SaveChangesAsync();
            return Ok(user.ID);
        }
    }
}
