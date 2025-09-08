using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Semana5.Data;
using Semana5.Model;

namespace Semana5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ServerDbContext _context;

        public UsuariosController(ServerDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuariosModel>>> GetUsuariosModel()
        {
            return await _context.UsuariosModel.ToListAsync();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuariosModel>> GetUsuariosModel(int id)
        {
            var usuariosModel = await _context.UsuariosModel.FindAsync(id);

            if (usuariosModel == null)
                return NotFound();

            return usuariosModel;
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuariosModel(int id, UsuariosModel usuariosModel)
        {
            if (id != usuariosModel.Id)
                return BadRequest();

            _context.Entry(usuariosModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuariosModelExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<UsuariosModel>> PostUsuariosModel(UsuariosModel usuariosModel)
        {
            _context.UsuariosModel.Add(usuariosModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuariosModel", new { id = usuariosModel.Id }, usuariosModel);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuariosModel request)
        {
            if (request == null) return BadRequest();

            var correoInput = request.Correo?.Trim().ToLower();
            var pwdInput = request.pwd?.Trim();

            var user = await _context.UsuariosModel
                .FirstOrDefaultAsync(u => u.Correo.ToLower().Trim() == correoInput
                                       && u.pwd.Trim() == pwdInput);

            if (user == null) return Unauthorized();

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddHours(2),
                SameSite = SameSiteMode.None,
                Secure = false
            };

            Response.Cookies.Append("sessionUser", user.Nombre, cookieOptions);

            return Ok(new { user.Id, user.Nombre, user.Correo });
        }


        // GET: api/Usuarios/me
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            if (!Request.Cookies.TryGetValue("sessionUser", out var usuarioNombre))
                return Unauthorized();

            var user = await _context.UsuariosModel
                .FirstOrDefaultAsync(u => u.Nombre == usuarioNombre);

            if (user == null)
                return Unauthorized();

            return Ok(new { user.Id, user.Nombre, user.Correo });
        }

        // POST: api/Usuarios/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            if (Request.Cookies.ContainsKey("sessionUser"))
                Response.Cookies.Delete("sessionUser");

            return Ok();
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuariosModel(int id)
        {
            var usuariosModel = await _context.UsuariosModel.FindAsync(id);
            if (usuariosModel == null)
                return NotFound();

            _context.UsuariosModel.Remove(usuariosModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuariosModelExists(int id)
        {
            return _context.UsuariosModel.Any(e => e.Id == id);
        }
    }
}

