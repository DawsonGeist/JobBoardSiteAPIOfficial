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
using System;
using Microsoft.Extensions.Primitives;

namespace JobBoardSiteAPIOfficial.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class ApplicationUploadController : ControllerBase
    {
        [HttpPost("{user_id}")]
        public string? UploadFile(string user_id)
        {
            Console.WriteLine("Uploading File for " + user_id.ToString());
            var success = false;
            foreach (var file in HttpContext.Request.Form.Files)
            {
                if (file != null && file.Length > 0)
                {
                    string dir = (Directory.GetCurrentDirectory().ToString() + "/Uploads/Test/" + user_id + "/" + file.Name);
                    Directory.CreateDirectory(dir);

                    Console.WriteLine(file.Name);
                    //Need to think about how we want store documents, "File Structure"
                    StreamWriter sw = new StreamWriter(dir + "/" + file.FileName);
                    file.CopyTo(sw.BaseStream);
                    sw.Close();
                    success = true;
                }
            }

            return success ? "Upload Successful" : null;
        }
    }
}
