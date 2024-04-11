using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Web.Http;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;

namespace JobBoardSiteAPIOfficial.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class ApplicationUploadController : ControllerBase
    {
        [HttpPost]
        public string? UploadFile()
        {
            Console.WriteLine("Uploading File");
            var file = HttpContext.Request.Form.Files.Count > 0 ?
                HttpContext.Request.Form.Files[0] : null;

            if (file != null && file.Length > 0)
            {
                StreamWriter sw = new StreamWriter((Directory.GetCurrentDirectory().ToString() + "/Uploads/Test/" + file.FileName));
                file.CopyTo(sw.BaseStream);
                sw.Close();
            }

            return file != null ? "Upload Successful" : null;
        }
    }
}
