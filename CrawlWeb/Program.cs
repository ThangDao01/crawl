using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrawlWeb.Vnexpress;
using HtmlAgilityPack;

namespace CrawlWeb
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            CrawlController crawlController = new CrawlController();
            Console.WriteLine("----Vui lòng chờ 1 phút để khởi động chương trình ");
            List<Link> listLinkTopicVnex = crawlController.GetTopicLink("https://vnexpress.net", 0);
            while (true)
            {
                int MaxTopic = 5;
                string choiceStr = "";
                int choiceNum = -1;
                while (choiceStr == "")
                {
                    if (listLinkTopicVnex.Count < MaxTopic)
                    {
                        MaxTopic = listLinkTopicVnex.Count;
                    }

                    Console.WriteLine("---------- Danh sách Chủ đề ----------");
                    for (int i = 0; i < MaxTopic; i++)
                    {
                        Console.Write((i + 1) + "." + listLinkTopicVnex[i].Topic + "\n");
                    }
                    if (listLinkTopicVnex.Count > MaxTopic)

                    {
                        Console.WriteLine("--- Enter để hiện thêm ");
                    }
                    MaxTopic += 5;
                    Console.Write("Chọn chủ đề muốn xem :");
                    choiceStr = Console.ReadLine();
                }

                if (choiceStr != "")
                {
                    choiceNum = Convert.ToInt32(choiceStr) - 1;
                }
                
                Console.WriteLine("Loading . . .");
                crawlController.CrawlListLink(listLinkTopicVnex, choiceNum);
                Console.WriteLine("---Enter để tiếp tục ");
                Console.ReadLine();
            }
        }
    }
}