using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ApiReto.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PreguntasController : ControllerBase
    {
        public string ConnectionString = "Server=127.0.0.1;Port=3306;Database=aventura_oxxo;Uid=root;password=root;";

        [HttpGet("facil/{id}")]
        public ActionResult<Pregunta> GetPreguntaFacil(int id)
        {
            return GetPreguntaPorDificultad(id, "Facil");
        }

        [HttpGet("dificil/{id}")]
        public ActionResult<Pregunta> GetPreguntaDificil(int id)
        {
            return GetPreguntaPorDificultad(id, "Dificil");
        }

        private ActionResult<Pregunta> GetPreguntaPorDificultad(int id, string dificultad)
        {
            Pregunta pregunta = null;

            using (MySqlConnection conexion = new MySqlConnection(ConnectionString))
            {
                conexion.Open();

                string query = @"SELECT * FROM pregunta WHERE ID_Pregunta = @id AND Dificultad = @dificultad";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@dificultad", dificultad);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            pregunta = new Pregunta()
                            {
                                ID_Pregunta = Convert.ToInt32(reader["ID_Pregunta"]),
                                Texto = reader["Texto"].ToString(),
                                OpcionA = reader["OpcionA"].ToString(),
                                OpcionB = reader["OpcionB"].ToString(),
                                OpcionC = reader["OpcionC"].ToString(),
                                OpcionD = reader["OpcionD"].ToString(),
                                RespuestaCorrecta = reader["RespuestaCorrecta"].ToString(),
                                Dificultad = reader["Dificultad"].ToString()
                            };
                        }
                    }
                }
            }

            if (pregunta == null)
                return NotFound();

            return Ok(pregunta);
        }
    }
}