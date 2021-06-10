using System;
using System.Collections.Generic;
using System.Linq;
using CrawlWeb.Vnexpress;
using HtmlAgilityPack;

namespace CrawlWeb.Vnexpress
{
    public class CrawlController
    {
        public void CrawlDetail(string url)
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(url);
            var listNode = htmlDoc.DocumentNode.SelectNodes("//h3[contains(@class,'title-news')]/a");
            Article.ListArticle.Clear();
            string check1 = "https://video";
            string check2 = ".html";
            for (int i = 0; i < listNode.Count; i++)
            {
                if (listNode.ElementAt(i).Attributes["href"].Value.StartsWith(check1) ||
                    listNode.ElementAt(i).Attributes["href"].Value.LastIndexOf(".html") == -1)
                {
                    continue;
                }
                else
                {
                    Article.ListArticle.Add(CrawlArticle(listNode.ElementAt(i).Attributes["href"].Value));
                }
            }

            ShowListArticle();
        }

        public Article CrawlArticle(string url)
        {
            Article article = new Article();
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(url);
            article.Link = url;
            article.Title = htmlDoc.DocumentNode.SelectSingleNode("//h1[contains(@class,'title-detail')]").InnerText;
            article.Time = htmlDoc.DocumentNode.SelectSingleNode("//span[contains(@class,'date')]").InnerText;
            // article.Content= htmlDoc.DocumentNode.SelectSingleNode("//article[contains(@class,'fck_detail')]").InnerHtml;
            var listcontent = htmlDoc.DocumentNode.SelectNodes("//p");
            for (int i = 0; i < listcontent.Count; i++)
            {
                article.Content += listcontent[i].InnerHtml;
            }
            var listImg = htmlDoc.DocumentNode.SelectNodes("//img");
            string checkImg = "data:image/gif;";
            for (int i = 0; i < listImg.Count; i++)
            {
                if (listImg[i].Attributes["src"].Value.StartsWith(checkImg) ||listImg[i].Attributes["src"].Value.Length==0)
                {
                }
                else
                {
                    article.LinkImg += "<Link img>:" + listImg[i].Attributes["src"].Value + "\n";
                }
            }
            return article;
        }

        public void ShowListArticle()
        {
            for (int i = 0; i < Article.ListArticle.Count; i++)
            {
                Console.WriteLine("---Article "+i);
                Console.WriteLine("<Link>:"+Article.ListArticle[i].Link);
                Console.WriteLine("<Time>:"+Article.ListArticle[i].Time);
                Console.WriteLine("<Title>"+Article.ListArticle[i].Title);
                Console.WriteLine(Article.ListArticle[i].Content);
                Console.WriteLine(Article.ListArticle[i].LinkImg);
                Console.WriteLine("----------------------------------------------------------------------------------");
            }
        }

        public List<Link> GetTopicLink(string mainUrl, int count)
        {
            var listLink = new List<Link>();
            var web = new HtmlWeb();
            var doc = web.Load(mainUrl);
            var listNode =
                doc.DocumentNode.SelectNodes("//nav[contains(@class,'main-nav')]/ul[contains(@class,'parent')]/li/a");
            if (listNode.Count < count)
            {
                Console.WriteLine("--- Giới hạn {0} chủ đề !! ", listNode.Count);
            }
            else if (count == 0)
            {
                count = listNode.Count;
            }

            for (int i = 0; i < listNode.Count; i++)
            {
                if (listNode.ElementAt(i).Attributes["href"].Value.Length > 2 &&
                    listNode.ElementAt(i).InnerText.Length > 2 &&
                    i != 5 && i != 20)
                {
                    Link link = new Link();
                    link.Url = mainUrl + listNode.ElementAt(i).Attributes["href"].Value;
                    link.Topic = listNode.ElementAt(i).InnerHtml;
                    listLink.Add(link);
                }
                else
                {
                    count++;
                }
            }

            return listLink;
        }

        public void CrawlListLink(List<Link> listLink, int choice)
        {
            if (choice == -1)
            {
                for (int i = 0; i < listLink.Count; i++)
                {
                    CrawlDetail(listLink[i].Url);
                }
            }
            else
            {
                CrawlDetail(listLink[choice].Url);
            }
        }
    }
}