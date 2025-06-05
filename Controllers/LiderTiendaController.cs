using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ApiReto.Controllers;



 [Route("[controller]")]

public class LiderTiendaController : ControllerBase
{
    public string ConnectionString ="Server=127.0.0.1;Port=3306;Database=aventura_oxxo;Uid=root;password=root;";


[HttpGet("{idUsuario}")]
        public ActionResult<LiderTienda> GetLider(int idUsuario)
        {
            LiderTienda lider = null;

            using (var conexion = new MySqlConnection(ConnectionString))
            {
                conexion.Open();
                string query = @"
                    SELECT ID_LiderTienda, ID_Usuario, Nivel, Diamantes,  Foto_de_Perfil 
                    FROM lidertienda 
                    WHERE ID_Usuario = @id";

                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@id", idUsuario);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lider = new LiderTienda
                            {
                                ID_LiderTienda = Convert.ToInt32(reader["ID_LiderTienda"]),
                                ID_Usuario     = Convert.ToInt32(reader["ID_Usuario"]),
                                Nivel          = Convert.ToInt32(reader["Nivel"]),
                                Diamantes      = Convert.ToInt32(reader["Diamantes"]),
                                Foto_de_Perfil = reader["Foto_de_Perfil"]?.ToString()
                            };
                        }
                    }
                }
            }

            if (lider == null)
                return NotFound();

            return Ok(lider);
        }

        // PUT /LiderTienda/{idUsuario}
        [HttpPut("{idUsuario}")]
        public IActionResult UpdateDiamantes(
            int idUsuario,
            [FromBody] LiderTienda liderBody)
        {
            using (var conexion = new MySqlConnection(ConnectionString))
            {
                conexion.Open();
                string update = @"
                    UPDATE lidertienda
                    SET Diamantes = @diamantes
                    WHERE ID_Usuario = @id";

                using (var cmd = new MySqlCommand(update, conexion))
                {
                    cmd.Parameters.AddWithValue("@diamantes", liderBody.Diamantes);
                    cmd.Parameters.AddWithValue("@id", idUsuario);

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    if (filasAfectadas == 0)
                        return NotFound();
                }
            }

            return NoContent();
        }
    }
