using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Tls;
using System.Collections.Generic;

namespace ApiReto.Controllers;

[ApiController]
[Route("[controller]")]
public class UsuariosController : ControllerBase
{
    public string ConnectionString = "Server=127.0.0.1;Port=3306;Database=aventura_oxxo;Uid=root;password=root;";

    [HttpGet]
    public IEnumerable<usuario> Get()
    {
        List<usuario> usuarios = new List<usuario>();

        using (MySqlConnection conexion = new MySqlConnection(ConnectionString))
        {
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM usuario", conexion);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    usuario usr = new usuario();
                    usr.ID_Usuario = Convert.ToInt32(reader["ID_Usuario"]);
                    usr.Nombre = reader["Nombre"].ToString();
                    usr.Usuario = reader["Usuario"].ToString();
                    usr.Contrasena = reader["Contrasena"].ToString();
                    usr.Foto_de_Perfil = reader["Foto_de_Perfil"].ToString();

                    usr.ID_Cargo = reader["ID_Cargo"] != DBNull.Value ? Convert.ToInt32(reader["ID_Cargo"]) : 0;

                    usr.Fecha_CuentaAgregada = Convert.ToDateTime(reader["Fecha_CuentaAgregada"]);
                    usr.Idioma = reader["Idioma"].ToString();

                    usuarios.Add(usr);
                }
            }
        }

        return usuarios;
    }


    [HttpGet("{id}")]
    public usuario Get(int id)
    {
        usuario usr = new usuario();

        using (MySqlConnection conexion = new MySqlConnection(ConnectionString))
        {
            conexion.Open();

            string query = @"SELECT ID_Usuario, Nombre, Usuario, Contrasena, Foto_de_Perfil, ID_Cargo, ID_AsesorTienda, ID_LiderTienda, Fecha_CuentaAgregada, Idioma  FROM usuario WHERE ID_Usuario = @id";

            MySqlCommand cmd = new MySqlCommand(query, conexion);
            cmd.Parameters.AddWithValue("@id", id);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())

                {
                    usr.ID_Usuario = Convert.ToInt32(reader["ID_Usuario"]);
                    usr.Nombre = reader["Nombre"]?.ToString();
                    usr.Usuario = reader["Usuario"]?.ToString();
                    usr.Contrasena = reader["Contrasena"]?.ToString();
                    usr.Foto_de_Perfil = reader["Foto_de_Perfil"]?.ToString();
                    usr.ID_Cargo = reader["ID_Cargo"] != DBNull.Value ? Convert.ToInt32(reader["ID_Cargo"]) : 0;
                    usr.Fecha_CuentaAgregada = Convert.ToDateTime(reader["Fecha_CuentaAgregada"]);
                    usr.Idioma = reader["Idioma"]?.ToString();
                }

            }


        }

        return usr;
    }


[HttpGet("login/{usuario}/{contrasena}")]
public ActionResult<usuario> Login( string usuario, string contrasena)
{
    using (MySqlConnection conexion = new MySqlConnection(ConnectionString))
    {
        conexion.Open();

        string query = @"
            SELECT ID_Usuario,Nombre, Usuario, Contrasena,Foto_de_Perfil,ID_Cargo,Fecha_CuentaAgregada,Idioma
            FROM usuario
            WHERE Usuario    = @user
              AND Contrasena = @pass";

        using (MySqlCommand cmd = new MySqlCommand(query, conexion))
        {
            cmd.Parameters.AddWithValue("@user", usuario);
            cmd.Parameters.AddWithValue("@pass", contrasena);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (!reader.Read())
                    return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos" });

                usuario usr = new usuario
                {
                    ID_Usuario           = Convert.ToInt32(reader["ID_Usuario"]),
                    Nombre               = reader["Nombre"].ToString(),
                    Usuario              = reader["Usuario"].ToString(),
                    Contrasena           = reader["Contrasena"].ToString(),
                    Foto_de_Perfil       = reader["Foto_de_Perfil"].ToString(),
                    ID_Cargo             = reader["ID_Cargo"] != DBNull.Value 
                                              ? Convert.ToInt32(reader["ID_Cargo"]) 
                                              : 0,
                    Fecha_CuentaAgregada = Convert.ToDateTime(reader["Fecha_CuentaAgregada"]),
                    Idioma               = reader["Idioma"].ToString()
                };

                return Ok(usr);
            }
        }
    }
}

}