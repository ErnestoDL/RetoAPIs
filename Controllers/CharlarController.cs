using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using ApiReto;

namespace ApiReto.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharlarController : ControllerBase
    {
        private const string ConnectionString =
            "Server=127.0.0.1;Port=3306;Database=aventura_oxxo;Uid=root;password=Hemaan,33;";

        [HttpGet]
        public ActionResult<IEnumerable<Charlar>> GetAllCharlas()
        {
            var charlas = new List<Charlar>();
            using var conn = new MySqlConnection(ConnectionString);
            conn.Open();

            using var cmd = new MySqlCommand(
                "SELECT ID_Charlar, Texto FROM charlar", conn);
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                charlas.Add(new Charlar
                {
                    ID_Charlar = rdr.GetInt32("ID_Charlar"),
                    Texto      = rdr.GetString("Texto")
                });
            }

            return Ok(charlas);
        }
    }
}
