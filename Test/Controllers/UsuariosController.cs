using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Model;

namespace Test.Controllers
{
    [Route("api/Usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly PruebaTI _context;

        public UsuariosController(PruebaTI context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Usuario>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                return Ok(await _context.Usuarios.ToListAsync());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
                return StatusCode(400, ModelState);
            }
        }

        [HttpGet("{Id:int}", Name = "GetBook")]
        [ProducesResponseType(200, Type = typeof(Usuario))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetUser(int Id)
        {
            try
            {
                Usuario user = await _context.Usuarios.FindAsync(Id);

                if (user == null)
                {
                    return NotFound("Este libro no existe.");
                }
                else
                {
                    return Ok(user);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
                return StatusCode(400, ModelState);
            }
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Usuario))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser([FromBody] Usuario user)
        {
            try
            {
                //check if object comes null
                if (user == null)
                {
                    return BadRequest(ModelState);
                }

                await _context.Usuarios.AddAsync(user);

                if (await CommitChanges())
                {
                    return Created("~api/Usuarios", new { user = user });
                }

                ModelState.AddModelError("", $"Algo salio mal al guardar el usuario {user.Nombre}");
                return StatusCode(500, ModelState);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
                return StatusCode(400, ModelState);
            }
        }

        [HttpPut(Name = "UpdateUser")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser([FromBody] Usuario user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest(ModelState);
                }

                if (_context.Entry(user).State == EntityState.Detached)
                {
                    _context.Attach(user);
                }
                _context.Entry(user).State = EntityState.Modified;

                if (!await CommitChanges())
                {
                    ModelState.AddModelError("", $"Algo salio mal al actualizar el usuario {user.Nombre}");
                    return StatusCode(500, ModelState);
                }

                return Content("Registro actualizado.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
                return StatusCode(400, ModelState);
            }
        }

        [HttpDelete("{Id:int}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser(int Id)
        {
            var objectToDelete = await _context.Usuarios.FindAsync(Id);
            _context.Usuarios.Remove(objectToDelete);
            return Ok(await CommitChanges());
        }



        private async Task<bool> CommitChanges()
        {
            if (await _context.SaveChangesAsync() >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
