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
using JobBoardSiteAPIOfficial.Services;

namespace JobBoardSiteAPIOfficial.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class ApplicationUploadController : ControllerBase
    {
        [HttpPost("{user_id}")]
        public List<Dictionary<string, string>> UploadFile(string user_id)
        {
            List<Dictionary<string, string>> content = new List<Dictionary<string, string>>();
            Console.WriteLine("Uploading File for " + user_id.ToString());
            var success = false;
            foreach (var file in HttpContext.Request.Form.Files)
            {
                if (file != null && file.Length > 0)
                {
                    string dir = (Directory.GetCurrentDirectory().ToString() + "/Uploads/Test/" + user_id + "/" + file.Name);
                    Directory.CreateDirectory(dir);

                    Console.WriteLine(file.Name);
                    string filepath = dir + "/" + file.FileName;
                    //Need to think about how we want store documents, "File Structure"
                    StreamWriter sw = new StreamWriter(filepath);
                    file.CopyTo(sw.BaseStream);
                    sw.Close();
                    success = true;


                    // Extract Resume Content
                    DocumentContentExtractorService dces = new DocumentContentExtractorService(DocumentContentExtractorService.DocumentType.RESUME, filepath);
                    content.Add(dces.Extract());
                }
            }


            return content;
        }
    }
}
