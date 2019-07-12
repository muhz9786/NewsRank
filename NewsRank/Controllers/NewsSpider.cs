// ================================
// Name: NewsSpider
// Description: 爬虫，爬取目标网站信息并存放在数据库;
//                     通过DbContext操作数据库，需要EF Core;
//                     爬虫策略编写在Update方法中;
// Author: Muhz
// Create Date: 2019-07-03
// ================================

using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NewsRank.Models;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace NewsRank.Controllers
{
    public class NewsSpider : Spider
    {
        /// <summary>
        /// Initializes a new Spider with configuration.
        /// </summary>
        /// <param name="configuration">IConfiguration</param>
        public NewsSpider(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Sipder running.
        /// </summary>
        public override void Run()
        {
            foreach (var web in configuration.GetSection("WebDict").GetChildren())
                Update(web.Value, web.Key);
        }

        /// <summary>
        /// Gets news and updates database records.
        /// </summary>
        /// <param name="url">target web URL.</param>
        /// <param name="type">target web category.</param>
        private void Update(string url, string type)
        {
            // Gets DbContext.
            var builder = new DbContextOptionsBuilder<NewsDBContext>();
            var options = builder.UseMySql(configuration.GetConnectionString("Default")).Options;
            NewsDBContext context = new NewsDBContext(options);
            var recordsSet = context.TblNews;    // DbSet of DbContext.

            HtmlDocument htmlDoc = GetHtmlDoc(url);    // HTML Document.

            // Get TAG nodes.
            var nodes = htmlDoc.DocumentNode.SelectNodes(
                "//div[@class = 'tabContents active']/table/tr");

            // Get news contents.
            if (nodes != null)
            {
                int index = 0;
                foreach (var node in nodes)
                {
                    if (index == 0)
                    { // First node is table head, skip it.
                        index++;
                        continue;
                    }

                    var newsNode = node.Element("td").Element("a");    // News contents.

                    try
                    { // If record is only one.
                        var record = recordsSet.Single(n =>     // Query a record that need update in set.
                                n.NewsRank == index && n.NewsType == type);

                        record.NewsUrl = newsNode.GetAttributeValue("href", null);    // URL

                        var doc = GetHtmlDoc(record.NewsUrl);
                        var content = doc.DocumentNode.SelectSingleNode(
                                "//div[@class = 'post_content_main']");
                        var title = content.Element("h1").InnerText;
                        var text = content.SelectSingleNode(
                            "//div[@class = 'post_body']/div[@class = 'post_text']")
                            .SelectNodes("p");
                        string str = "";
                        foreach (var p in text)
                            str = str + "<p>" + p.InnerHtml + "</p>";
                        /* Get source.
                        var source = content.SelectSingleNode("//div[@class = 'post_time_source']");
                        source.RemoveChild(source.SelectSingleNode("//a[@class = 'post_jubao']"));
                        Console.WriteLine(source.InnerText.Trim());
                         */

                        record.NewsTitle = title;    // Title.
                        record.NewsContent = str;    // Content.
                        record.SubmitTime = DateTime.Now;    // Submit time.

                        context.SaveChanges();    // save update.
                    }
                    catch (Exception)
                    { // record is more or less than one.
                        break;
                    }

                    if (index == 10) break;    // Get only 10  pieces of news.

                    index++;
                } // foreach (var node in nodes) ---
            } // if (nodes != null) ---
            else
            { // Getting nodes failed.
                Console.WriteLine("ERROR!");
            }
        } // private void Update(string url, string type) ---

        /// <summary>
        /// Gets a HtmlDocument.
        /// </summary>
        /// <param name="url">target Web URL</param>
        /// <returns>HTML Document</returns>
        private HtmlDocument GetHtmlDoc(string url)
        {
            // Register ecoding provider.
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // Get HTML.
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader streamReader = new StreamReader(
                response.GetResponseStream(),
                Encoding.GetEncoding(configuration.GetValue<string>("Encoding")));
            string html = streamReader.ReadToEnd();

            // HTML Parser.
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            return htmlDoc;
        } // private HtmlDocument GetHtmlDoc(string url) ---
    } // public class Spider ---
} // namespace NewsRank.Controllers ---
