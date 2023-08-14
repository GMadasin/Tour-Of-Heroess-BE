
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Base.Controllers
{
    [Route("api/Powers")]
    [ApiController]
    public class PowerController : ControllerBase
    {
        [HttpGet("{id}")]//gets power by id
        public ActionResult Get(int id)
        {
            string connectionString = "Server=localhost;Database=model;User Id=sa;Password=potato451.exe;MultipleActiveResultSets=true; encrypt=false";

            SqlConnection conCon = new SqlConnection(connectionString);
            conCon.Open();
            using (SqlCommand command = new SqlCommand($"SELECT ID, NAME FROM POWERS WHERE ID = @Id", conCon))
            {
                command.Parameters.Add(new SqlParameter("Id", id));
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return Ok(new Power { Id = Convert.ToInt32(reader[0]), Name = reader[1].ToString() });
                    }
                }
                
            }
            return Ok(new Power { Id = -1, Name = "dfsd"});
        }

        [HttpGet("AllPowers")]//returns array of all powers
        public ActionResult GetAllPowers()
        {
            string connectionString = "Server=localhost;Database=model;User Id=sa;Password=potato451.exe;MultipleActiveResultSets=true; encrypt=false";

            List<Power> allPowers = new List<Power>();
            using (SqlConnection conCon = new SqlConnection(connectionString))
            {
                conCon.Open();
                using (SqlCommand command = new SqlCommand($"SELECT ID, NAME FROM POWERS ORDER BY ID", conCon))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allPowers.Add(new Power { Id = Convert.ToInt32(reader[0]), Name = reader[1].ToString() });
                        }
                    }
                }
            }
            return Ok(allPowers);
        }

        [HttpGet("GetPower/{name}")]//search for the power by name
        public ActionResult GetPowerByName(string name)
        {
            string connectionString = "Server=localhost;Database=model;User Id=sa;Password=potato451.exe;MultipleActiveResultSets=true; encrypt=false";

            List<Power> allPowers = new List<Power>();
            using (SqlConnection conCon = new SqlConnection(connectionString))
            {
                conCon.Open();
                using (SqlCommand command = new SqlCommand($"SELECT ID, NAME FROM POWERS WHERE NAME LIKE @Name", conCon))
                {

                    command.Parameters.Add(new SqlParameter("Name", "%" + name + "%"));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allPowers.Add(new Power { Id = Convert.ToInt32(reader[0]), Name = reader[1].ToString() });
                        }
                    }
                }
            }
            return Ok(allPowers);
        }

        [HttpPost("AddPower")]//add power to table
        public ActionResult PostPower([FromBody] Power power)
        {
            string connectionString = "Server=localhost;Database=model;User Id=sa;Password=potato451.exe;MultipleActiveResultSets=true; encrypt=false";

            using (SqlConnection conCon = new SqlConnection(connectionString))
            {
                conCon.Open();
                using (SqlCommand command = new SqlCommand($"INSERT INTO POWERS (ID, NAME) VALUES (@Id, @Name)", conCon))
                {
                    command.Parameters.Add(new SqlParameter("Id", power.Id));
                    command.Parameters.Add(new SqlParameter("Name", power.Name));

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok(power); // return the created city object
                    }
                }
            }
            return StatusCode(500, "Unable to add the power to the database"); // return error if city couldn't be added
        }

        [HttpDelete("delete/{id}")]//deletes a power by id
        public ActionResult DeletePower(int id)
        {
            string connectionString = "Server=localhost;Database=model;User Id=sa;Password=potato451.exe;MultipleActiveResultSets=true; encrypt=false";

            using (SqlConnection conCon = new SqlConnection(connectionString))
            {
                conCon.Open();

                using (SqlCommand command = new SqlCommand($"DELETE FROM HERO_POWERS WHERE POWER_ID = @Id", conCon))
                {
                    command.Parameters.Add(new SqlParameter("Id", id));
                    command.ExecuteNonQuery();
                }

                using (SqlCommand command = new SqlCommand($"DELETE FROM POWERS WHERE ID = @Id", conCon))
                {
                    command.Parameters.Add(new SqlParameter("Id", id));
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return NoContent();
                    }
                }
            }

            return NotFound();
        }

        [HttpPost("UpdatePower")]//updates the power name
        public ActionResult UpdatePower([FromBody] Power power)
        {
            string connectionString = "Server=localhost;Database=model;User Id=sa;Password=potato451.exe;MultipleActiveResultSets=true; encrypt=false";

            using (SqlConnection conCon = new SqlConnection(connectionString))
            {
                conCon.Open();
                using (SqlCommand command = new SqlCommand($"UPDATE POWERS SET NAME = @Name WHERE ID = @Id", conCon))
                {
                    command.Parameters.Add(new SqlParameter("Id", power.Id));
                    command.Parameters.Add(new SqlParameter("Name", power.Name));

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok(power); 
                    }
                }
            }
            return StatusCode(500, "Unable to update the city in the database"); // return error if city couldn't be updated
        }
    }

}

