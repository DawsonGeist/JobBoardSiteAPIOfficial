using JobBoardSiteAPIOfficial.Services.DocumentContentExtractor;

namespace JobBoardSiteAPIOfficial.Services
{
    public class DocumentContentExtractorService
    {
        public enum DocumentType
        {
            RESUME,
            COVERLETTER
        }

        ResumeCrawler _crawler; // Change this to interface for crawlers. Then one variable can represent both the Coverletter and Resume Crawling

        public DocumentContentExtractorService(DocumentType documentType, string filepath) 
        {
            if(documentType == DocumentType.RESUME)
            {
                _crawler = new ResumeCrawler(filepath);
            }
        }

        public Dictionary<string, string> Extract() 
        {
            Dictionary<string,string> values = _crawler.Extract();
            foreach(var value in values) 
            {
                //Console.WriteLine(value.Key + ":");
                //Console.WriteLine(value.Value);
            }
            return values;
        }
    }
}
