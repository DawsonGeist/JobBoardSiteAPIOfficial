using JobBoardSiteAPIOfficial.DbContexts;
using JobBoardSiteAPIOfficial.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobBoardSiteAPIOfficial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Job_PostingController : ControllerBase
    {
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<Object>? Get()
        {
            List<Job_Posting>? jobs = null;
            List<Object> result = new List<Object>();
            //Get All Job Postings
            using (var db = new Job_PostingContext(new Microsoft.EntityFrameworkCore.DbContextOptions<Job_PostingContext>()))
            {
                db.OpenConnection();
                jobs = db.Jobs.ToList<Job_Posting>();
                db.CloseConnection();
            }

            if (jobs != null)
            {
                //Load in Company Data
                using (var db = new CompanyContext(new Microsoft.EntityFrameworkCore.DbContextOptions<CompanyContext>()))
                {
                    db.OpenConnection();
                    foreach (Job_Posting job in jobs)
                    {
                        Company? postingCompany = db.Companies.SingleOrDefault<Company>(e => e.company_id == job.company_id);
                        if (postingCompany != null)
                        {
                            Dictionary<string, Object?> partialResult = new Dictionary<string, Object?>();
                            partialResult.Add("posting", job);
                            partialResult.Add("company", postingCompany);
                            result.Add(partialResult);
                        }
                        else
                        {
                            //This shouldnt happen. Check Database
                            Console.WriteLine("Job Posting's company_id has no matching record in company table");
                        }
                    }
                    db.CloseConnection();
                }
            }

            return result;
        }

        // GET: api/<UserController>
        [HttpGet("{company_id}")]
        public IEnumerable<Object>? Get(string company_id) // Get job Postings by a specific company_id
        {
            List<Job_Posting>? jobs = null;
            //Get All Job Postings
            using (var db = new Job_PostingContext(new Microsoft.EntityFrameworkCore.DbContextOptions<Job_PostingContext>()))
            {
                db.OpenConnection();
                Console.WriteLine(company_id);
                foreach(Job_Posting job in db.Jobs)
                {
                    Console.WriteLine($"{job.company_id}");
                }
                jobs = db.Jobs.Where<Job_Posting>(e => e.company_id == company_id).ToList(); ;

                db.CloseConnection();
            }
            return jobs;
        }

        // POST api/<LoginController>
        [HttpPost()]
        public Job_Posting Post([FromBody] Job_Posting newJob)
        {
            using (var db = new Job_PostingContext(new Microsoft.EntityFrameworkCore.DbContextOptions<Job_PostingContext>()))
            {
                if (!db.OpenConnection())
                    throw new Exception("ERROR OPENING CONNECTION");
                Console.WriteLine("Create Job Posting Request Received");
                Console.WriteLine(newJob.job_title);

                Job_Posting? duplicate = db.Jobs.SingleOrDefault<Job_Posting>(e => (e.job_title == newJob.job_title && e.company_id == newJob.company_id));
                if (duplicate != null)
                {
                    Console.WriteLine("Duplicate Job Posting for this company found");
                    newJob.job_title = "Job Already Exists";
                }
                else
                {
                    db.Add(newJob);
                    db.SaveChanges();
                    db.CloseConnection();
                }

                return newJob;
            }
        }
    }
}
