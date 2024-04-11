using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardSiteAPIOfficial.Models
{
    [Table("company_employee")]
    [PrimaryKey("user_id", "company_id")]
    public class Company_Employee
    {
        public required string company_id { get; set; }

        public required string user_id { get; set; }

    }
}
