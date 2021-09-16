using Datos.MongoDB;
using Datos.MongoDB.Clases;
using Funcionalidad.Request;
using Funcionalidad.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcionalidad.Service
{
    public class Service
    {
        public readonly string _table = "NoSQL";
        public readonly string _collectionComentarios = "comentarios";
        public readonly string _collectionUsuarios = "usuarios";

        public void crearUsuario(UsuarioRequest user)
        {
            if (!existeUsuario(user.Email))
            {
                Usuario usuario = new Usuario
                {
                    CI = user.CI,
                    Nombre = user.Nombre,
                    Apellido = user.Apellido,
                    Email = user.Email
                };
                Mongodb mdb = new Mongodb(_table);
                mdb.InsertRecord(_collectionUsuarios, usuario);
            }
            else
            {
                throw (new Exception("Ya existe usuario con ese e-mail o CI"));
            }
        }
        private bool existeUsuario(string email)
        {
            Mongodb mdb = new Mongodb(_table);
            List<Usuario> usuarios = mdb.LoadRecord<Usuario>(_collectionUsuarios).ToList();
            foreach (Usuario usuario in usuarios)
            {
                if (usuario.Email.Equals(email))
                {
                    return true;
                }
            }
            return false;
        }
        private Usuario buscarUsuario(int ci, string email)
        {
            Mongodb mdb = new Mongodb(_table);
            List<Usuario> usuarios = mdb.LoadRecord<Usuario>(_collectionUsuarios).ToList();
            foreach (Usuario usuario in usuarios)
            {
                if (usuario.CI == ci && usuario.Email.Equals(email))
                {
                    return usuario;
                }
            }
            return null;
        }

        public void crearComentario(ComentarioRequest comentario)
        {
            if (comentario.Texto.Length < 256)
            {
                Usuario u = buscarUsuario(comentario.CIUser, comentario.EmailUser);
                if (u != null)
                {
                    Comentario c = new Comentario
                    {
                        Texto = comentario.Texto,
                        emisor = u,
                        Likes = new List<Like>()
                    };
                    try
                    {
                        Mongodb mdb = new Mongodb(_table);
                        mdb.InsertRecord(_collectionComentarios, c);
                    }
                    catch (Exception e)
                    {
                        throw (new Exception("Algo anduvo mal al intentar agregar un comentario, fijarse en la conexion a la base de datos"));
                    }

                }
                else
                {
                    throw (new Exception("No se encuentra usuario o email"));
                }
            }
            else
            {
                throw (new Exception("Comentario demasiado largo"));
            }
        }
        public List<ComentarioResponse> listarComentariosUsuario(string email)
        {
            List<ComentarioResponse> listaretorno = new List<ComentarioResponse>();
            try
            {
                Mongodb mdb = new Mongodb(_table);
                List<Comentario> comentarios = mdb.LoadRecord<Comentario>(_collectionComentarios).ToList();
                if (!existeUsuario(email))
                {
                    throw (new Exception("No existe el usuario"));
                }
                foreach (Comentario c in comentarios)
                {
                    if (c.emisor.Email.Equals(email))
                    {
                        int cantLike = 0, cantDislike = 0;
                        if (c.Likes != null)
                        {
                            foreach (Like l in c.Likes)
                            {
                                if (l.like == true)
                                {
                                    cantLike++;
                                }
                                else
                                {
                                    cantDislike++;
                                }
                            }
                        }
                        ComentarioResponse cr = new ComentarioResponse
                        {
                            Id = c.Id.ToString(),
                            Texto = c.Texto,
                            emisor = c.emisor.CI + " (" + c.emisor.Nombre + " " + c.emisor.Apellido + ")",
                            FechaEnviado = c.FechaEnviado,
                            CantLikes = cantLike,
                            CantDislikes = cantDislike
                        };
                        listaretorno.Add(cr);
                    }
                }
                return listaretorno;
            }
            catch (Exception e)
            {
                throw (new Exception("Algo anduvo mal al intentar listar los comentarios, fijarse en la conexion a la base de datos"));
            }
        }

        public void agregarLike(bool like, Guid idComentario, string email)
        {
            try
            {
                Mongodb mdb = new Mongodb(_table);
                List<Usuario> usuarios = mdb.LoadRecord<Usuario>(_collectionUsuarios).ToList();
                Usuario u = null;
                foreach (var item in usuarios)
                {
                    if (item.Email.Equals(email))
                    {
                        u = item;
                    }

                }
                if (u == null)
                {
                    throw (new Exception("No existe ese usuario"));
                }
                List<Comentario> comentarios = mdb.LoadRecord<Comentario>(_collectionComentarios).ToList();
                foreach (Comentario c in comentarios)
                {
                    if (c.Id.Equals(idComentario))
                    {
                        Like l = new Like
                        {
                            like = like,
                            emisor = u
                        };
                        c.Likes.Add(l);
                        mdb.UpsertRecord(_collectionComentarios, c.Id, c);
                    }
                }
            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }


        }
        public ComentarioResponse retornarComentario(Guid idComentario)
        {
            try
            {
                Mongodb mdb = new Mongodb(_table);
                List<Comentario> comentarios = mdb.LoadRecord<Comentario>(_collectionComentarios).ToList();
                foreach (Comentario c in comentarios)
                {
                    if (c.Id.Equals(idComentario))
                    {
                        int cantLike = 0, cantDislike = 0;
                        if (c.Likes != null)
                        {
                            foreach (Like l in c.Likes)
                            {
                                if (l.like == true)
                                {
                                    cantLike++;
                                }
                                else
                                {
                                    cantDislike++;
                                }
                            }
                        }
                        return new ComentarioResponse
                        {
                            Id = c.Id.ToString(),
                            Texto = c.Texto,
                            emisor = c.emisor.CI + " (" + c.emisor.Nombre + " " + c.emisor.Apellido + ")",
                            FechaEnviado = c.FechaEnviado,
                            CantLikes = cantLike,
                            CantDislikes = cantDislike
                        };
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                throw (new Exception("Algo salio mal con el acceso a la base"));
            }


        }
    }
}
