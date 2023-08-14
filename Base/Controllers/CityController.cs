using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Base.Controllers
{
    [Route("api/Cities")]
    [ApiController]
    public class CityController : ControllerBase
    {
        [HttpGet("{id}")]//get city by id
        public ActionResult Get(int id)
        {
            string connectionString = "Server=localhost;Database=model;User Id=sa;Password=potato451.exe;MultipleActiveResultSets=true; encrypt=false";

            SqlConnection conCon = new SqlConnection(connectionString);
            conCon.Open();
            using (SqlCommand command = new SqlCommand($"SELECT ID, NAME FROM CITY WHERE ID = @Id", conCon))
            {
                command.Parameters.Add(new SqlParameter("Id", id));
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return Ok(new City { Id = Convert.ToInt32(reader[0]), Name = reader[1].ToString() });
                    }
                }

            }
            return Ok(new City { Id = -1, Name = "none" });
        }

        [HttpDelete("delete/{id}")]//delete city by id
        public ActionResult DeleteCity(int id)
        {
            string connectionString = "Server=localhost;Database=model;User Id=sa;Password=potato451.exe;MultipleActiveResultSets=true; encrypt=false";

            SqlConnection conCon = new SqlConnection(connectionString);
            conCon.Open();
            using (SqlCommand command = new SqlCommand($"DELETE FROM CITY WHERE ID = @Id", conCon))
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


        [HttpGet("AllCities")]//gets all cities
        public ActionResult GetAllCities()
        {
            string connectionString = "Server=localhost;Database=model;User Id=sa;Password=potato451.exe;MultipleActiveResultSets=true; encrypt=false";

            List<City> allCities = new List<City>();
            using (SqlConnection conCon = new SqlConnection(connectionString))
            {
                conCon.Open();
                using (SqlCommand command = new SqlCommand($"SELECT ID, NAME FROM CITY ORDER BY ID", conCon))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allCities.Add(new City { Id = Convert.ToInt32(reader[0]), Name = reader[1].ToString() });
                        }
                    }
                }
            }
            return Ok(allCities);
        }

        [HttpGet("GetCity/{name}")]//searches for city by name
        public ActionResult GetCityByName(string name)
        {
            string connectionString = "Server=localhost;Database=model;User Id=sa;Password=potato451.exe;MultipleActiveResultSets=true; encrypt=false";

            List<City> allCities = new List<City>();
            using (SqlConnection conCon = new SqlConnection(connectionString))
            {
                conCon.Open();
                using (SqlCommand command = new SqlCommand($"SELECT ID, NAME FROM CITY WHERE NAME LIKE @Name", conCon))
                {
                    // The '%' before and after the name variable is a wildcard that matches any number of characters
                    command.Parameters.Add(new SqlParameter("Name", "%" + name + "%"));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allCities.Add(new City { Id = Convert.ToInt32(reader[0]), Name = reader[1].ToString() });
                        }
                    }
                }
            }
            return Ok(allCities);
        }

        [HttpPost("AddCity")]//add city 
        public ActionResult PostCity([FromBody] City city)
        {
            string connectionString = "Server=localhost;Database=model;User Id=sa;Password=potato451.exe;MultipleActiveResultSets=true; encrypt=false";

            using (SqlConnection conCon = new SqlConnection(connectionString))
            {
                conCon.Open();
                using (SqlCommand command = new SqlCommand($"INSERT INTO CITY (ID, NAME) VALUES (@Id, @Name)", conCon))
                {
                    command.Parameters.Add(new SqlParameter("Id", city.Id));
                    command.Parameters.Add(new SqlParameter("Name", city.Name));

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok(city);
                    }
                }
            }

            return StatusCode(500, "Unable to add the city to the database");
        }

        [HttpPost("UpdateCity")]//updates the city
        public ActionResult UpdateCity([FromBody] City city)
        {
            string connectionString = "Server=localhost;Database=model;User Id=sa;Password=potato451.exe;MultipleActiveResultSets=true; encrypt=false";

            using (SqlConnection conCon = new SqlConnection(connectionString))
            {
                conCon.Open();

                using (SqlCommand command = new SqlCommand($"UPDATE CITY SET NAME = @Name WHERE ID = @Id", conCon))
                {
                    command.Parameters.Add(new SqlParameter("Id", city.Id));
                    command.Parameters.Add(new SqlParameter("Name", city.Name));

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok(city);
                    }
                }

                return StatusCode(500, "Unable to update the city in the database");
            }
        }
    }
}
