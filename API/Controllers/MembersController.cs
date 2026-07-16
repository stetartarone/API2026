using Microsoft.AspNetCore.Mvc;
using API.Entities;
using Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    public class MembersController(AppDbContext context) : BaseApiController
    {
        [HttpGet]
        // GET: api/Members
        public async Task<ActionResult<IReadOnlyList<AppUser>>> GetMembers()
        {
            // Restetuisce tutti i membri presenti nel database
            var members = await context.Users.ToListAsync();
            return members;
        }
        [Authorize]
        [HttpGet("{id}")]
        // GET: api/Members/5
        public async Task<ActionResult<AppUser>> GetMember(string id)
        {
            // Restituisce un membro specifico in base all'ID fornito
            var member = await context.Users.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return member;
        }
    }
}