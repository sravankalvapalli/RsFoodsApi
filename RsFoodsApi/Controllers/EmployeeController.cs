using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RsFoodsApi.Models;
using System.Data.SqlClient;

namespace RsFoodsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetEmployees()
        {

            var sqlConnection = _configuration.GetConnectionString("SqlConnectionString");

            var emps = new List<Employee>();
            using (SqlConnection sqlConn = new SqlConnection(sqlConnection))
            {
                sqlConn.Open();
                SqlCommand cmd = sqlConn.CreateCommand();
                cmd.CommandText = "Select * From Employee;";
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    var employee = new Employee()
                    {
                        FirstName = sqlDataReader["FirstName"]?.ToString() ?? "",
                        LastName = sqlDataReader["LastName"]?.ToString() ?? "",
                        Phone = sqlDataReader["Phone"]?.ToString() ?? "",
                    };
                    emps.Add(employee);
                }

            }

            emps.Add(new Employee { FirstName = "test firstname", LastName = "test lastname", Phone = "000-000-0000" });
            return Ok(emps);

        }
    }
}
