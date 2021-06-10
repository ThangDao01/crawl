using System.Collections.Generic;

namespace CrawlWeb.Vnexpress
{
    public class Article
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string LinkImg { get; set; }
        public string Time { get; set; }
        public static List<Article> ListArticle = new List<Article>();
    }
}