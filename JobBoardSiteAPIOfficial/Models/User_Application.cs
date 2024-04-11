using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardSiteAPIOfficial.Models
{
    [Table("user_application")]
    [PrimaryKey("user_id", "company_id", "job_title")]
    public class User_Application
    { 
        public required string user_id { get; set; }
        public required string company_id { get; set; }
        public required string job_title { get; set; }
    }
}
