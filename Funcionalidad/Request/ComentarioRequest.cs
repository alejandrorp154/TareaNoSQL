using System;
using System.Collections.Generic;
using System.Text;

namespace Funcionalidad.Request
{
    public class ComentarioRequest
    {
        public string Texto { get; set; }
        public string EmailUser { get; set; }
        public int CIUser { get; set; }
    }
}
