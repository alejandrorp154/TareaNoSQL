using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funcionalidad.Request;
using Funcionalidad.Response;
using Funcionalidad.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NoSQL.Controllers
{
    [Route("api/foro")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private Service service;

        public ServiceController()
        {
            service = new Service();
        }
        [HttpPost("usuario/crear")]
        public IActionResult crearUsuario(UsuarioRequest usuarioRequest)
        {
            try
            {
                service.crearUsuario(usuarioRequest);
                return Ok(new { mensaje = "Usuario Creado" });
            }catch(Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpPost("comentario/crear")]
        public IActionResult crearComentario(ComentarioRequest comentarioRequest)
        {
            try
            {
                service.crearComentario(comentarioRequest);
                return Ok(new { mensaje = "Comentario Creado" });
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpGet("comentario/listar")]
        public IActionResult listarComentariosUsuario(string email)
        {
            try
            {
                List<ComentarioResponse> lcr= service.listarComentariosUsuario(email);
                return Ok(new { comentarios = lcr });
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpGet("comentario/obtener")]
        public IActionResult obtenerComentario (string idComentario)
        {
            try
            {
                Guid guid = new Guid(idComentario);
                ComentarioResponse cr= service.retornarComentario(guid);
                return Ok(new { comentario = cr });
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message});
            }
        }

        [HttpPost("comentario/like")]
        public IActionResult agregarLike(bool like, string idComentario, string email)
        {
            try
            {

                Guid guid = new Guid(idComentario);
                //string guid = Guid.Parse(idComentario).ToString("D");
                service.agregarLike(like, guid, email);
                return Ok(new { mensaje = "Like Agregado" });
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }
    }
}
