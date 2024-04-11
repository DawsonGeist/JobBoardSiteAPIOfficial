using JobBoardSiteAPIOfficial.Models;
using Microsoft.EntityFrameworkCore;

namespace JobBoardSiteAPIOfficial.DbContexts
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using JobBoardSiteAPIOfficial.Models;

    public class Job_PostingContext : DbContext
    {
        public DbSet<Job_Posting> Jobs { get; set; }

        static readonly string connectionString = "Server=localhost; User ID=root; Password=password; Database=job_board_schema";

        public Job_PostingContext(DbContextOptions<Job_PostingContext> options) : base(options)
        {

        }

        public bool OpenConnection()
        {
            bool success = false;
            if (Database.CanConnect())
            {
                this.Database.OpenConnection();
                success = true;
            }
            return success;
        }

        public void CloseConnection()
        {
            Database.CloseConnectionAsync().Wait();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

    }
}
