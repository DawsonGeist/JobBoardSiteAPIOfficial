using JobBoardSiteAPIOfficial.DbContexts;
using JobBoardSiteAPIOfficial.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardSiteAPIOfficial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        // POST api/<LoginController>
        [HttpPost()]
        public Login Post([FromBody] Login newLogin)
        {
            using (var db = new LoginContext(new Microsoft.EntityFrameworkCore.DbContextOptions<LoginContext>()))
            {
                if (!db.OpenConnection())
                    throw new Exception("ERROR OPENING CONNECTION");
                Console.WriteLine("Create Login Request Received");
                Console.WriteLine(newLogin.ToString());
                db.Add(newLogin);
                db.SaveChanges();
                db.CloseConnection();

                return newLogin;
            }
        }

        // GET api/<LoginController>/
        [HttpGet("{email}/{pass}")]
        public Object? Get(string email, string pass)
        {
            Login? requestedLogin = null;
            User? requestedUser = null;
            Company_Employee? employeeRecord = null;
            Company? company = null;
            Dictionary<string, Object?> result = new Dictionary<string, Object?>();

            using (var db = new LoginContext(new Microsoft.EntityFrameworkCore.DbContextOptions<LoginContext>()))
            {
                // Verify Credentials
                requestedLogin = db.Logins.SingleOrDefault<Login>(e => (e.email == email && e.password == pass));
            }

            if (requestedLogin != null)
            {
                // Get User Entity
                using (var db = new UserContext(new Microsoft.EntityFrameworkCore.DbContextOptions<UserContext>()))
                {
                    requestedUser = db.Users.SingleOrDefault<User>(e => e.user_id == requestedLogin.user_id);
                    result.Add("user", requestedUser);
                }
            }

            if (requestedUser != null)
            {
                // Check for company
                using (var db = new Company_EmployeeContext(new Microsoft.EntityFrameworkCore.DbContextOptions<Company_EmployeeContext>()))
                {
                    employeeRecord = db.Employees.SingleOrDefault<Company_Employee>(e => e.user_id == requestedUser.user_id);
                }
            }
            else
            {
                // This shouldn't happen if we have login credentials. Check database records
                return null;
            }

            if(employeeRecord != null)
            {
                // Get the User's company
                using (var db = new CompanyContext(new Microsoft.EntityFrameworkCore.DbContextOptions<CompanyContext>()))
                {
                    company = db.Companies.SingleOrDefault<Company>(e => e.company_id == employeeRecord.company_id);
                    result.Add("company", company);
                }
            }

            return result;
        }
    }
}
