using JobBoardSiteAPIOfficial.Models;
using Microsoft.EntityFrameworkCore;

namespace JobBoardSiteAPIOfficial.DbContexts
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using JobBoardSiteAPIOfficial.Models;

    public class LoginContext : DbContext
    {
        public DbSet<Login> Logins { get; set; }

        static readonly string connectionString = "Server=localhost; User ID=root; Password=password; Database=job_board_schema";

        public LoginContext(DbContextOptions<LoginContext> options) : base(options)
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
