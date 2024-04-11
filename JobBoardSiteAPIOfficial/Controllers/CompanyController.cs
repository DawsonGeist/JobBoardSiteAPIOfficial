using JobBoardSiteAPIOfficial.DbContexts;
using JobBoardSiteAPIOfficial.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardSiteAPIOfficial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        // POST api/<LoginController>
        [HttpPost()]
        public Company Post([FromBody] Company newCompany)
        {
            using (var db = new CompanyContext(new Microsoft.EntityFrameworkCore.DbContextOptions<CompanyContext>()))
            {
                if (!db.OpenConnection())
                    throw new Exception("ERROR OPENING CONNECTION");
                Console.WriteLine("Create Company Request Received");
                Console.WriteLine(newCompany.name);

                Company? duplicate = db.Companies.SingleOrDefault<Company>(e => e.name.ToLower() == newCompany.name.ToLower());
                if(duplicate != null)
                {
                    Console.WriteLine("Duplicate Company Name Found");
                    newCompany.name = "Duplicate Name";
                }
                else
                {
                    newCompany.company_id = Guid.NewGuid().ToString();
                    db.Add(newCompany);
                    db.SaveChanges();
                    db.CloseConnection();
                }

                return newCompany;
            }
        }
    }
}
