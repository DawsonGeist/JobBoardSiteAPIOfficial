using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardSiteAPIOfficial.Models
{
    [Table("job_posting")]
    [PrimaryKey("company_id", "job_title")]
    public class Job_Posting
    {
        public required string company_id { get; set; }

        public required string job_title { get; set; }

        public string? description { get; set; }

        public required int is_active { get; set; }
    }
}
