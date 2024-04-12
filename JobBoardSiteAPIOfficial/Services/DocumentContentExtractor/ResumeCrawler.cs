using System.Runtime.CompilerServices;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
using UglyToad.PdfPig;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Primitives;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace JobBoardSiteAPIOfficial.Services.DocumentContentExtractor
{
    //The sole purpose of this class is to crawl through the resume and extract possible key values
    public class ResumeCrawler
    {
        private ResumeDumper _dumper;
        public ResumeCrawler(string filepath) 
        {
            _dumper = new ResumeDumper(filepath);
        }

        public Dictionary<string, string> Extract()
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            string content = _dumper.Dump();
            var sections = aiParse(content).Result;
            values.Add("resume", sections);
            return values;
        }

        private async Task<string> aiParse(string content)
        {
            Console.WriteLine("Parsing");
            var client = new HttpClient();
            var payload = new Dictionary<string, object>();
            var messages = new List<Dictionary<string, object>>();
            var systemMessage = new Dictionary<string, object>();
            var userMessage = new Dictionary<string, object>();

            payload.Add("model", "gpt-3.5-turbo");

            systemMessage.Add("role", "system");
            systemMessage.Add("content", "parse the incoming resume string and try to extract the Personal Summary, Relevant Skills, Education History, Work History, Volunteer History, Personal Projects, Awards. Put your response in strict JSON format (With Keys Personal_Summary, Relevant_Skills (Array of individual skills, no additional organization), Education_History (Array of dictionaries with keys (Degree, Subject, Graduation_Date, University)), Work_History (Values are dictionaries with keys (Title, Company, Dates, Responsibilities(Array of strings))), Volunteer_History (Values are dictionaries with keys (Title, Company, Dates, Responsibilities)), Personal_Projects (Values are dictionaries with keys (Title, Dates, Description)), Awards (Values are dictionaries with keys (Title, Dates, Description))). Remove JSON violating characters from your response. replace null values with empty arrays.");

            messages.Add(systemMessage);

            userMessage.Add("role", "user");
            userMessage.Add("content", content);

            messages.Add(userMessage);

            payload.Add("messages", messages);

            // Create the HttpContent for the form to be posted.
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "");//Fix this hide the api key like a man
            req.Content = JsonContent.Create(payload);
            req.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");


            // Get the response.
            HttpResponseMessage response = await client.SendAsync(req);

            Console.WriteLine($"Response: {response.StatusCode}");


            // Get the response content.
            HttpContent responseContent = response.Content;
            var x = await responseContent.ReadAsStringAsync();
            return x;
        }

        private class ResumeDumper
        {
            private string _filepath;
            public ResumeDumper(string filepath)
            {
                _filepath = filepath;
            }

            public string Dump() 
            {
                string content = "";
                if(File.Exists(_filepath))
                {
                    if(_filepath.Substring(_filepath.LastIndexOf('.')) == ".pdf")
                    {
                        using (var pdf = PdfDocument.Open(_filepath)) //https://dev.to/eliotjones/reading-a-pdf-in-c-on-net-core-43ef
                        {
                            foreach (var page in pdf.GetPages())
                            {
                                // Either extract based on order in the underlying document with newlines and spaces.
                                var text = ContentOrderTextExtractor.GetText(page);

                                // Or based on grouping letters into words.
                                //var otherText = string.Join(" ", page.GetWords());

                                // Or the raw text of the page's content stream.
                                //var rawText = page.Text;

                                content += text;
                            }

                        }
                    }
                    else
                    {
                        try
                        {
                            StreamReader sr = new StreamReader(_filepath);
                            content = sr.ReadToEnd();
                            sr.Close();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("FilePath does not exist");
                }
                return content;
            }
        }
    }
}
