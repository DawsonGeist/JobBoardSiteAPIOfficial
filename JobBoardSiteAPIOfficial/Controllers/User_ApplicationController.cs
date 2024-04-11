using JobBoardSiteAPIOfficial.DbContexts;
using JobBoardSiteAPIOfficial.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/ef/language-reference/query-expression-syntax-examples-join-operators
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Data.Objects;
using System.Globalization;
//using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Data.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Collections.Immutable;
//

namespace JobBoardSiteAPIOfficial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class User_ApplicationController : ControllerBase
    {
        // POST api/<LoginController>
        [HttpPost()]
        public User_Application Post([FromBody] User_Application newApplication)
        {
            using (var db = new User_ApplicationContext(new Microsoft.EntityFrameworkCore.DbContextOptions<User_ApplicationContext>()))
            {
                if (!db.OpenConnection())
                    throw new Exception("ERROR OPENING CONNECTION");
                Console.WriteLine("Create Job Application Request Received");

                User_Application? duplicate = db.Applications.SingleOrDefault<User_Application>(e => (e.job_title == newApplication.job_title && e.company_id == newApplication.company_id && e.user_id == newApplication.user_id));
                if (duplicate != null)
                {
                    Console.WriteLine("Duplicate Job Application for this (company, job) found");
                    newApplication.job_title = "Application Already Exists";
                }
                else
                {
                    db.Add(newApplication);
                    db.SaveChanges();
                    db.CloseConnection();
                }

                return newApplication;
            }
        }

        // GET api/<LoginController>
        [HttpGet("{company_id}/{job_title}")]
        public IEnumerable<Object> GetUserApplicationsForTargetJobPosting(string company_id, string job_title)
        {
            List<User_Application> apps = null;
            List<Dictionary<string, Object?>> result = new List<Dictionary<string, Object?>>();
            using (var db = new User_ApplicationContext(new Microsoft.EntityFrameworkCore.DbContextOptions<User_ApplicationContext>()))
            {
                if (!db.OpenConnection())
                    throw new Exception("ERROR OPENING CONNECTION");
                Console.WriteLine("Get Job Applications Request Received (company, job_title)");

                apps = db.Applications.Where<User_Application>(e => (e.company_id == company_id && e.job_title == job_title)).ToList<User_Application>();
            }

            using (var db = new UserContext(new Microsoft.EntityFrameworkCore.DbContextOptions<UserContext>()))
            {
                foreach(User_Application app in apps)
                {
                    Dictionary<string, Object> partialResult = new Dictionary<string, Object>();
                    partialResult.Add("application", app);
                    partialResult.Add("user", db.Users.SingleOrDefault<User>(e => e.user_id == app.user_id));
                    result.Add(partialResult);
                }
            }

            return result;
        }


        // GET api/<LoginController>
        [HttpGet("{user_id}")]
        public IEnumerable<Object> GetUserApplicationsByUserId(string user_id)
        {
            List<User_Application> apps;
            List<Dictionary<string, Object?>> result = new List<Dictionary<string, Object?>>();
            using (var db = new User_ApplicationContext(new Microsoft.EntityFrameworkCore.DbContextOptions<User_ApplicationContext>()))
            {
                if (!db.OpenConnection())
                    throw new Exception("ERROR OPENING CONNECTION");
                Console.WriteLine("Get Job Applications Request Received (user_id)");

                apps = db.Applications.Where<User_Application>(e => e.user_id == user_id).ToList<User_Application>();
            }

            /*// Cant use multiple Context's ... Have to put these all into one context before you can do a query
            using (var db1 = new User_ApplicationContext(new Microsoft.EntityFrameworkCore.DbContextOptions<User_ApplicationContext>()))
            {
                using (var db2 = new CompanyContext(new Microsoft.EntityFrameworkCore.DbContextOptions<CompanyContext>()))
                {
                    using (var db3 = new Job_PostingContext(new Microsoft.EntityFrameworkCore.DbContextOptions<Job_PostingContext>()))
                    {
                        var query =
                            from application in db1.Applications
                            join company_job_posting in (from company in db2.Companies
                                                         join job_posting in db3.Jobs
                                                         on company.company_id equals job_posting.company_id
                                                         select new
                                                         {
                                                             company_id = company.company_id,
                                                             company_name = company.name,
                                                             job_title = job_posting.job_title,
                                                             description = job_posting.description
                                                         })
                            on application.company_id equals company_job_posting.company_id
                            where application.job_title == company_job_posting.job_title && application.company_id == company_job_posting.company_id && application.user_id == user_id
                            select new
                            {
                                company_id = company_job_posting.company_id,
                                company_name = company_job_posting.company_name,
                                job_title = company_job_posting.job_title,
                                description = company_job_posting.description
                            };
                        return query.ToList();
                    }
                }
            }*/
            foreach (User_Application app in apps)
            {
                Dictionary<string, Object?> partialResult = new Dictionary<string, Object?>();
                //Load in Company Data
                using (var db = new CompanyContext(new Microsoft.EntityFrameworkCore.DbContextOptions<CompanyContext>()))
                {
                    db.OpenConnection();
                    Company? postingCompany = db.Companies.SingleOrDefault<Company>(e => e.company_id == app.company_id);
                    if (postingCompany != null)
                    {
                        partialResult.Add("application", app);
                        partialResult.Add("company", postingCompany);
                    }
                    else
                    {
                        //This shouldnt happen. Check Database
                        Console.WriteLine("Application's company_id has no matching record in company table");
                    }
                    db.CloseConnection();
                }

                //Load in Job data
                using (var db3 = new Job_PostingContext(new Microsoft.EntityFrameworkCore.DbContextOptions<Job_PostingContext>()))
                {
                    db3.OpenConnection();
                    Job_Posting? post = db3.Jobs.SingleOrDefault<Job_Posting>(job => job.job_title == app.job_title && job.company_id == app.company_id);
                    if(post != null)
                    {
                        partialResult.Add("post", post);

                    }
                    else
                    {
                        //This shouldnt happen. Check Database
                        Console.WriteLine("Application's (company_id, Job_Title) has no matching record in job_posting");
                    }

                }

                result.Add(partialResult);
            }

            return result;
        }
    }
}
