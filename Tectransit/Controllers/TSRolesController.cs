using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tectransit.Modles;

namespace Tectransit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TSRolesController : ControllerBase
    {
        private readonly TECTRANSITDBContext _context;

        public TSRolesController(TECTRANSITDBContext context)
        {
            _context = context;
        }

        // GET: api/TSRoles
        [HttpGet]
        public IEnumerable<TSRole> GetTSRole()
        {
            return _context.TSRole;
        }

        // GET: api/TSRoles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTSRole([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tSRole = await _context.TSRole.FindAsync(id);

            if (tSRole == null)
            {
                return NotFound();
            }

            return Ok(tSRole);
        }

        // PUT: api/TSRoles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTSRole([FromRoute] long id, [FromBody] TSRole tSRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tSRole.Id)
            {
                return BadRequest();
            }

            _context.Entry(tSRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TSRoleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TSRoles
        [HttpPost]
        public async Task<IActionResult> PostTSRole([FromBody] TSRole tSRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TSRole.Add(tSRole);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTSRole", new { id = tSRole.Id }, tSRole);
        }

        // DELETE: api/TSRoles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTSRole([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tSRole = await _context.TSRole.FindAsync(id);
            if (tSRole == null)
            {
                return NotFound();
            }

            _context.TSRole.Remove(tSRole);
            await _context.SaveChangesAsync();

            return Ok(tSRole);
        }

        private bool TSRoleExists(long id)
        {
            return _context.TSRole.Any(e => e.Id == id);
        }
    }
}