using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos.MongoDB.Clases
{
    public class Usuario
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public int CI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
    }
}
