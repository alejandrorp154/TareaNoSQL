using System;
using System.Collections.Generic;
using System.Text;

namespace Funcionalidad.Response
{
    public class ComentarioResponse
    {
        public string Id { get; set; }
        public string emisor { get; set; }
        public int CantLikes { get; set; }
        public int CantDislikes { get; set; }
        public string Texto { get; set; }
        public DateTime FechaEnviado { get; set; }
    }
}
