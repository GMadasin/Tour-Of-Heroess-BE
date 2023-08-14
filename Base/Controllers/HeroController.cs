using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Base.Controllers
{
    [Route("api/Heroes")]
    [ApiController]
    public class HeroController : ControllerBase
    {
        [HttpGet("{id}")]//gets hero by id
        public ActionResult Get(int id)
        {
            string connectionString = "Server=localhost;Database=model;User Id=sa;Password=potato451.exe;MultipleActiveResultSets=true; encrypt=false";

            SqlConnection conCon = new SqlConnection(connectionString);
            conCon.Open();
            using (SqlCommand command = new SqlCommand($"SELECT ID, NAME, CITY_ID FROM HERO WHERE ID = @Id", conCon))
            {
                command.Parameters.Add(new SqlParameter("Id", id));
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return Ok(new Hero { Id = Convert.ToInt32(reader[0]), Name = reader[1].ToString(), CityId = Convert.ToInt32(reader[2]) });
                    }
                }

            }
            return Ok(new Hero { Id = -1, Name = "dfsd", CityId = -1 });
        }

        [HttpGet("AllHeroes")]//returns an array of all heroes
        public ActionResult GetAllHeroes()
        {
            string connectionString = "Server=localhost;Database=model;User Id=sa;Password=potato451.exe;MultipleActiveResultSets=true; encrypt=false";

            List<Hero> allHeroes = new List<Hero>();
            using (SqlConnection conCon = new SqlConnection(connectionString))
            {
                conCon.Open();
                using (SqlCommand command = new SqlCommand($"SELECT ID, NAME, CITY_ID FROM HERO ORDER BY ID", conCon))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allHeroes.Add(new Hero { Id = Convert.ToInt32(reader[0]), Name = reader[1].ToString(), CityId = Convert.ToInt32(reader[2]) });
                        }
                    }
                }
            }
            return Ok(allHeroes);
        }

        [HttpDelete("delete/{id}")]//deletes a hero by id
        public ActionResult DeleteHero(int id)
        {
            string connectionString = "Server=localhost;Database=model;User Id=sa;Password=potato451.exe;MultipleActiveResultSets=true; encrypt=false";

            SqlConnection conCon = new SqlConnection(connectionString);
            conCon.Open();
            using (SqlCommand command = new SqlCommand($"DELETE FROM HERO WHERE ID = @Id", conCon))
            {
                command.Parameters.Add(new SqlParameter("Id", id));
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return NoContent();
                }
            }
            return NotFound();
        }
        [HttpGet("GetHero/{name}")]//searches for hero by name
        public ActionResult GetHeroByName(string name)
        {
            string connectionString = "Server=localhost;Database=model;User Id=sa;Password=potato451.exe;MultipleActiveResultSets=true; encrypt=false";

            List<Hero> allHeroes = new List<Hero>();
            using (SqlConnection conCon = new SqlConnection(connectionString))
            {
                conCon.Open();
                using (SqlCommand command = new SqlCommand($"SELECT ID, NAME, CITY_ID FROM HERO WHERE NAME LIKE @Name", conCon))
                {
                    // The '%' before and after the name variable is a wildcard that matches any number of characters
                    command.Parameters.Add(new SqlParameter("Name", "%" + name + "%"));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allHeroes.Add(new Hero { Id = Convert.ToInt32(reader[0]), Name = reader[1].ToString(), CityId = Convert.ToInt32(reader[2]) });
                        }
                    }
                }
            }
            return Ok(allHeroes);
        }

        [HttpPost("AddHero")]//add hero to table
        public ActionResult PostHero([FromBody] Hero hero)
        {
            string connectionString = "Server=localhost;Database=model;User Id=sa;Password=potato451.exe;MultipleActiveResultSets=true; encrypt=false";

            using (SqlConnection conCon = new SqlConnection(connectionString))
            {
                conCon.Open();
                using (SqlCommand command = new SqlCommand($"INSERT INTO HERO (ID, NAME, CITY_ID) VALUES (@Id, @Name, @CityId)", conCon))
                {
                    command.Parameters.Add(new SqlParameter("Id", hero.Id));
                    command.Parameters.Add(new SqlParameter("Name", hero.Name));
                    command.Parameters.Add(new SqlParameter("CityId", hero.CityId));

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok(new Hero { Id = hero.Id, Name = hero.Name, CityId = hero.CityId });
                    }
                }
            }
            return StatusCode(500, "Unable to add the hero to the database");
        }

        [HttpPost("UpdateHero")]//updates the hero
        public ActionResult UpdateHero([FromBody] Hero hero)
        {
            string connectionString = "Server=localhost;Database=model;User Id=sa;Password=potato451.exe;MultipleActiveResultSets=true; encrypt=false";

            using (SqlConnection conCon = new SqlConnection(connectionString))
            {
                conCon.Open();
                using (SqlCommand command = new SqlCommand($"UPDATE HERO SET NAME = @Name, CITY_ID = @CityId  WHERE ID = @Id", conCon))
                {
                    command.Parameters.Add(new SqlParameter("Id", hero.Id));
                    command.Parameters.Add(new SqlParameter("Name", hero.Name));
                    command.Parameters.Add(new SqlParameter("CityId", hero.CityId));

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok(hero); // return the updated city object
                    }
                }
            }
            return StatusCode(500, "Unable to update the hero in the database"); // return error if city couldn't be updated
        }

        [HttpPost("AddPower/{heroId}/{powerId}")]
        public ActionResult PostHeroPower(int heroId, int powerId)
        {
            string connectionString = "Server=localhost;Database=model;User Id=sa;Password=potato451.exe;MultipleActiveResultSets=true; encrypt=false";

            using (SqlConnection conCon = new SqlConnection(connectionString))
            {
                conCon.Open();
                using (SqlCommand command = new SqlCommand($"INSERT INTO HERO_POWERS (HERO_ID, POWER_ID) VALUES (@HId, @PId)", conCon))
                {
                    command.Parameters.Add(new SqlParameter("HId", heroId));
                    command.Parameters.Add(new SqlParameter("PId", powerId));

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok();
                    }
                }
            }
            return StatusCode(500, "Unable to add the hero to the database");
        }

        [HttpDelete("DeletePower/{heroId}/{powerId}")]
        public ActionResult DeleteHeroPower(int heroId, int powerId)
        {
            string connectionString = "Server=localhost;Database=model;User Id=sa;Password=potato451.exe;MultipleActiveResultSets=true; encrypt=false";

            using (SqlConnection conCon = new SqlConnection(connectionString))
            {
                conCon.Open();
                using (SqlCommand command = new SqlCommand($"DELETE FROM HERO_POWERS WHERE HERO_ID = @HId AND POWER_ID = @PId", conCon))
                {
                    command.Parameters.Add(new SqlParameter("HId", heroId));
                    command.Parameters.Add(new SqlParameter("PId", powerId));

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok();
                    }
                }
            }
            return StatusCode(500, "Unable to delete the hero power from the database");
        }

        [HttpGet("GetPowers/{heroId}")]
        public ActionResult GetHeroPowers(int heroId)
        {
            string connectionString = "Server=localhost;Database=model;User Id=sa;Password=potato451.exe;MultipleActiveResultSets=true; encrypt=false";

            List<Power> allPowers = new List<Power>();

            using (SqlConnection conCon = new SqlConnection(connectionString))
            {
                conCon.Open();

                // Fetching power IDs first
                List<int> powerIds = new List<int>();
                using (SqlCommand command = new SqlCommand($"SELECT POWER_ID FROM HERO_POWERS WHERE HERO_ID = @HId", conCon))
                {
                    command.Parameters.Add(new SqlParameter("HId", heroId));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            powerIds.Add(Convert.ToInt32(reader[0]));
                        }
                    }
                }

                // Now, fetch the names and IDs of these powers
                foreach (int powerId in powerIds)
                {
                    using (SqlCommand command = new SqlCommand($"SELECT ID, NAME FROM POWERS WHERE ID = @PId", conCon))
                    {
                        command.Parameters.Add(new SqlParameter("PId", powerId));

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                allPowers.Add(new Power { Id = Convert.ToInt32(reader[0]), Name = reader[1].ToString() });
                            }
                        }
                    }
                }
            }
            return Ok(allPowers);
        }
    }
}

