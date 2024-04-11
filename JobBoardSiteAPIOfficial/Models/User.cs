using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardSiteAPIOfficial.Models
{
    [Table("user")]
    [PrimaryKey("user_id")]
    public class User
    {
        [Column(TypeName = "varchar(50)")]
        public string? user_id { get; set; }

        public required string first_name { get; set; }

        public required string last_name { get; set; }

        public required string email { get; set; }

        public required string phone { get; set; }
    }
}
