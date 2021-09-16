using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos.MongoDB.Clases
{
    public class Comentario
    {
        [BsonId]
        public Guid Id { get; set; }
        public Usuario emisor { get; set; }
        public List<Like> Likes { get; set; }
        public string Texto { get; set; }
        public DateTime FechaEnviado { get; set; }

        public Comentario()
        {
            FechaEnviado = DateTime.Now;
        }

    }
}
