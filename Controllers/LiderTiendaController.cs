using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ApiReto.Controllers;



[Route("[controller]")]

public class LiderTiendaController : ControllerBase
{
    public string ConnectionString = "Server=127.0.0.1;Port=3306;Database=aventura_oxxo;Uid=root;password=;";


    [HttpGet("{idUsuario}")]
    public ActionResult<LiderTienda> GetLider(int idUsuario)
    {
        LiderTienda lider = null;

        using (var conexion = new MySqlConnection(ConnectionString))
        {
            conexion.Open();
            string query = @"
                    SELECT ID_LiderTienda, ID_Usuario, Nivel, Diamantes
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
                            ID_Usuario = Convert.ToInt32(reader["ID_Usuario"]),
                            Nivel = Convert.ToInt32(reader["Nivel"]),
                            Diamantes = Convert.ToInt32(reader["Diamantes"]),
                        };
                    }
                }
            }
        }

        if (lider == null)
            return NotFound();

        return Ok(lider);
    }
    // GET /LiderTienda/top5
    [HttpGet("top5")]
    public ActionResult<List<object>> GetTop5Lideres()
    {
        var listaTop5 = new List<object>();

        using (var conexion = new MySqlConnection(ConnectionString))
        {
            conexion.Open();
            string query = @"
            SELECT
              lt.Apodo,
              lt.Diamantes,
              u.Usuario,
              DENSE_RANK() OVER (ORDER BY lt.Diamantes DESC) AS Posicion
            FROM lidertienda lt
            JOIN usuario u ON u.ID_Usuario = lt.ID_Usuario
            ORDER BY lt.Diamantes DESC
            LIMIT 5;";

            using (var cmd = new MySqlCommand(query, conexion))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var lider = new
                    {
                        Apodo = reader["Apodo"].ToString(),
                        Usuario = reader["Usuario"].ToString(),
                        Diamantes = Convert.ToInt32(reader["Diamantes"]),
                        Posicion = Convert.ToInt32(reader["Posicion"])
                    };

                    listaTop5.Add(lider);
                }
            }
        }

        return Ok(listaTop5);
    }
    [HttpGet("rank/{idUsuario}")]
    public ActionResult<object> GetRank(int idUsuario)
    {
        using (var conexion = new MySqlConnection(ConnectionString))
        {
            conexion.Open();
            string query = @"
            SELECT ID_Usuario, Diamantes, Posicion
            FROM (
              SELECT 
                ID_Usuario,
                Diamantes,
                DENSE_RANK() OVER (ORDER BY Diamantes DESC) AS Posicion
              FROM lidertienda
            ) AS ranking
            WHERE ID_Usuario = @id;";

            using (var cmd = new MySqlCommand(query, conexion))
            {
                cmd.Parameters.AddWithValue("@id", idUsuario);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var resultado = new
                        {
                            ID_Usuario = Convert.ToInt32(reader["ID_Usuario"]),
                            Diamantes = Convert.ToInt32(reader["Diamantes"]),
                            Posicion = Convert.ToInt32(reader["Posicion"])
                        };

                        return Ok(resultado);
                    }
                }
            }
        }

        return NotFound();
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
