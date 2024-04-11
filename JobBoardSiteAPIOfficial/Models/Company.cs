using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardSiteAPIOfficial.Models
{
    [Table("company")]
    [PrimaryKey("company_id")]
    public class Company
    {
        public string? company_id { get; set; }

        public required string name { get; set; }
    }
}
