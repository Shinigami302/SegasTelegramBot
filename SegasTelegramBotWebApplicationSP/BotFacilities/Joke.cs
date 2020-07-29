using System;
using System.Net;
using System.Text;


namespace SegasTelegramBotWebApplicationSP
{

    public class Joke
    {
        private const string URL = "http://www.RzhuNeMogu.ru/Widzh/WidzhRNM.aspx?type=11";
        public string GetAJoke()
        {
            string htmlCode;
            Random r = new Random();
            string s = $"c {r.Next(0, 99999 + 1).ToString()}";

            using (WebClient client = new WebClient())
            {
                var ec = CodePagesEncodingProvider.Instance.GetEncoding(1251);

                htmlCode = ec.GetString(client.DownloadData($"{URL}&callback=callbacks.{s}"));
            }

            string html = htmlCode.Substring(htmlCode.IndexOf("result") + 9, htmlCode.IndexOf("href") - 35);
            string text = html.Replace("<br />", "\n");
            text = text.Replace("&quot;", "'").Replace("& quot;", "'");
            text = $"Вибачайте хлопці, що російською, але ось така суєта:\n\n{text}";
            return text;
        }
    }
}
