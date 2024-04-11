using Microsoft.AspNetCore.Mvc;

using JobBoardSiteAPIOfficial.DbContexts;
using JobBoardSiteAPIOfficial.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JobBoardSiteAPIOfficial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<User>? Get()
        {
            using (var db = new UserContext(new Microsoft.EntityFrameworkCore.DbContextOptions<UserContext>()))
            {
                db.OpenConnection();
                List<User>? users = db.Users?.ToList();
                db.CloseConnection();
                return users;

            }
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public User? Get(string id)
        {
            using (var db = new UserContext(new Microsoft.EntityFrameworkCore.DbContextOptions<UserContext>()))
            {
                User? requestedUser = db.Users.SingleOrDefault<User>(e => e.user_id == id);
                return requestedUser;
            }
        }

        // POST api/CreateUser
        [HttpPost()]
        public User Post([FromBody] User newUser)
        {
            using (var db = new UserContext(new Microsoft.EntityFrameworkCore.DbContextOptions<UserContext>()))
            {
                if (!db.OpenConnection())
                    throw new Exception("ERROR OPENING CONNECTION");
                // Check for duplicate email
                User? target = db.Users.FirstOrDefault(e => e.email == newUser.email);
                if (target != null)
                {
                    Console.WriteLine("Duplicate Email");
                    newUser.email = "Duplicate Email";
                }
                else
                {
                    newUser.user_id = Guid.NewGuid().ToString();

                    db.Add(newUser);
                    db.SaveChanges();
                    db.CloseConnection();
                }

                return newUser;
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            using (var db = new UserContext(new Microsoft.EntityFrameworkCore.DbContextOptions<UserContext>()))
            {
                if (!db.OpenConnection())
                    throw new Exception("ERROR OPENING CONNECTION");

                User? target = db.Users.SingleOrDefault(e => e.user_id == id);
                if (target != null)
                    db.Remove(target);

                db.SaveChanges();
                db.CloseConnection();
            }
        }
    }
}
