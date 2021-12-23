using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Linq;
using HtmlAgilityPack;
using System.IO;

namespace Proxys
{
    public static class ProxyClass
    {
        private static List<WebProxy> Proxys = new List<WebProxy>();// список проксей
        private static int NumPr = 0;//используемая сейчас прокси
        private static int QuerCnt = 0;//сколько запросов сделано с прокси
        private static DateTime lastUseNullProxy=DateTime.Now;//последнее использование без проксли
        private static int minutesForNullProxy = 90;// через какое время начинать обход списка с нуля
        /// <summary>
        /// меняет прокси (в try catch)
        /// </summary>
        public static void NexProxy()
        {
            if (NumPr == 0)
            {
                lastUseNullProxy = DateTime.Now;// меняет дату 
            } // если без прокси(0)
            if (QuerCnt == 1)
            {
                if (NumPr == 0) 
                {
                } // если без прокси(0)
                else
                {
                    Proxys.Remove(Proxys[NumPr]);
                    if (Proxys.Count == 1) { GetFromTxt(); }
                    NumPr--;
                }// если с прокси
            } //если был всего один запрос
            if (NumPr== Proxys.Count-1) 
            { 
                NumPr = 0;
            }//
            else 
            {
                NumPr++;
            }
            if (DateTime.Now.Subtract(lastUseNullProxy).TotalMinutes >= minutesForNullProxy)
            {
                NumPr = 0;
            }
            QuerCnt = 0;
        }
        /// <summary>
        /// возвращает прокси для использования
        /// </summary>
        /// <returns></returns>
        public static WebProxy GetProxy()
        {
            QuerCnt++;
            if (QuerCnt == 2 && NumPr!=0)
            {
                Write(append:true);
            }
            return Proxys[NumPr];
        }
        /// <summary>
        /// очищает список проксей
        /// </summary>
        public static void Clear()
        {
            Proxys.Clear();
        }
        /// <summary>
        /// записывает прокси в текстовик
        /// </summary>
        /// <param name="path">куда записывать</param>
        /// <param name="append"> дозаписать(true) в файл или новая запись</param>
        public static void Write(string path="goodProxy.txt", bool append = false )
        {
            using(StreamWriter sw=new StreamWriter(path, append, Encoding.UTF8))
            {
                if (Proxys[NumPr].Address!=null) { sw.WriteLine(Proxys[NumPr].Address.Authority); }
            }
        }
        /// <summary>
        ///  получае список проксей из текста
        /// </summary>
        /// <param name="path"></param>
        public static void GetFromTxt(string path="pr.txt")
        {
            Proxys.Clear();
            using(StreamReader sr=new StreamReader(path))
            {
                Proxys.Add(new WebProxy());
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] col = line.Split(':');
                    Proxys.Add(new WebProxy(line));
                }
                Proxys = Proxys.Distinct().ToList();
            }
            Proxys = Proxys.Distinct().ToList();
        }
        /// <summary>
        /// получае список проксей с сайта
        /// </summary>
        public static void GetFromSite()
        {
            Proxys.Clear();
            Proxys.Add(null);
            int start = 0, finish=0;
            do
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://hidemy.name/ru/proxy-list/?anon=4&start="+start+"#list");
                request.UserAgent = "Mozila/4.0 (compatible; MSIE 6.0;Windows NT 5.1; SV1; MyIE2; ";
                request.CookieContainer = new CookieContainer();
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8";
                request.Headers.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");
                request.Headers.Add("Sec-Fetch-Dest", "document");
                request.Headers.Add("Sec-Fetch-Mode", "navigate");
                request.Headers.Add("Sec-Fetch-Site", "same-origin");
                request.Headers.Add("TE", "trailers");
                request.Headers.Add("Upgrade-Insecure-Requests", "1");
                request.KeepAlive = true;
                HttpWebResponse response = null;
                response = (HttpWebResponse)request.GetResponse();
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.Load(response.GetResponseStream(), Encoding.UTF8);
                if (finish == 0)
                {
                    HtmlNodeCollection a = doc.DocumentNode.SelectNodes("//div[@class='pagination']")[0].SelectNodes(".//a");
                    foreach(HtmlNode htmlNode in a)
                    {
                        string url = htmlNode.GetAttributeValue("href", string.Empty);
                        if (!url.Contains("start")){continue; }
                        int valueSt = Convert.ToInt32(url.Replace("/ru/proxy-list/?anon=4&start=", "").Replace("#list", ""));
                        if(valueSt> finish) { finish= valueSt;}
                    }
                }

                HtmlNodeCollection rows = doc.DocumentNode.SelectNodes("//div[@class='table_block']")[0].SelectNodes(".//tbody")[0].SelectNodes(".//tr");
                foreach (HtmlNode row in rows)
                {
                    HtmlNodeCollection col = row.SelectNodes(".//td");
                    Proxys.Add(new WebProxy(col[0].InnerHtml + ":" + col[1].InnerHtml));
                }
                start += 32;
                //HtmlNodeCollection blocks = doc.DocumentNode.SelectNodes("//div[contains(@class, 'row blockInfo')]")[0].SelectNodes(".//section[contains(@class, 'blockInfo__section section')]");
            }
            while (start < finish);
            Proxys = Proxys.Distinct().ToList();
        }
        public enum Site
        {
            hidemyName
        }

    }
}
