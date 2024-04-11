using JobBoardSiteAPIOfficial.DbContexts;
using JobBoardSiteAPIOfficial.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardSiteAPIOfficial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Company_EmployeeController : ControllerBase
    {
        // POST api/<Company_EmployeeController>
        [HttpPost()]
        public Company_Employee Post([FromBody] Company_Employee newEmployee)
        {
            using (var db = new Company_EmployeeContext(new Microsoft.EntityFrameworkCore.DbContextOptions<Company_EmployeeContext>()))
            {
                if (!db.OpenConnection())
                    throw new Exception("ERROR OPENING CONNECTION");
                Console.WriteLine("Create Company Employee Request Received");

                Company_Employee? duplicate = db.Employees.SingleOrDefault<Company_Employee>(e => (e.user_id == newEmployee.user_id));
                if (duplicate != null)
                {
                    Console.WriteLine("User ID already Employeed");
                    newEmployee.company_id = "Already Employed";
                }
                else
                {
                    db.Add(newEmployee);
                    db.SaveChanges();
                    db.CloseConnection();
                }

                return newEmployee;
            }
        }
    }
}
