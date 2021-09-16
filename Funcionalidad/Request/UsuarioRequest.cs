using System;
using System.Collections.Generic;
using System.Text;

namespace Funcionalidad.Request
{
    public class UsuarioRequest
    {
        public int CI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
    }
}
