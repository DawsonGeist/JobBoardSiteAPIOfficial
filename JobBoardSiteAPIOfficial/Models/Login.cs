using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardSiteAPIOfficial.Models
{
    [Table("login_credentials")]
    [PrimaryKey("email", "password")]
    public class Login
    {
        public required string email { get; set; }

        public required string password { get; set; }

        public required string user_id { get; set; }
    }
}
