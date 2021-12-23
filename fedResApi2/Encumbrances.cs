using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;
using Proxys;
using System.Text.RegularExpressions;

namespace Encumbrance
{
    public class Encumbrances
    {
        public Encumbrances(string searchString)
        {
        linq1: HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://fedresurs.ru/backend/encumbrances?offset=0&limit=50&searchString=" + searchString);
            request.UserAgent = "Mozila/4.0 (compatible; MSIE 6.0;Windows NT 5.1; SV1; MyIE2; ";
            request.CookieContainer = new CookieContainer();
            request.Accept = "image/gif, image/x-xbitmap, image/jpeg,image / pjpeg, application / x - shockwave - flash, application / vnd.ms - excel, application / vnd.ms - powerpoint,  application / msword,";
            request.Headers.Add("Accept-Language", "ru");
            request.ContentType = "application/json";
            request.Referer = "fedresurs.ru";
            request.Proxy = ProxyClass.GetProxy();
            request.KeepAlive = true;
            request.Referer = "https://fedresurs.ru/backend/encumbrances?offset=0&limit=50&group=Leasing&searchString="+ Uri.EscapeDataString(searchString);
            HttpWebResponse response = null;
            //The remote server returned an error: (404) Not Found.
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException wEx)
            {
                switch (wEx.Message)
                {
                    case "Попытка установить соединение была безуспешной, т.к.от другого компьютера за требуемое время не получен нужный отклик, или было разорвано уже установленное соединение из-за неверного отклика уже подключенного компьютера.Попытка установить соединение была безуспешной, т.к.от другого компьютера за требуемое время не получен нужный отклик, или было разорвано уже установленное соединение из-за неверного отклика уже подключенного компьютера.":
                        ProxyClass.NexProxy();
                        goto linq1;
                    case "The remote server returned an error: (404) Not Found.":
                        var b = 0;
                        return;
                    default:
                        var a = 0;
                        break;
                }
                ProxyClass.NexProxy();
                goto linq1;

            }
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(response.GetResponseStream(), Encoding.UTF8);
            JObject jObject = JObject.Parse(doc.Text);
            this.searchString = searchString;
            if (jObject["pageData"] != null) { this.pageData = jObject["pageData"].ToObject<List<PageData>>(); }
            if (jObject["found"] != null) { this.found = jObject["found"].ToObject<int>(); }
            //"Заключение договора финансовой аренды (лизинга)"
            //PageData PD = pageData.FirstOrDefault(r => r.type == "Заключение договора финансовой аренды (лизинга)" && r.mainInfo.Contains(searchString));
            //PD.MainInfo = new MainInfo(PD.mainInfo);
            //List<PageData> pageData2 = pageData.Where(r => r.type == "Заключение договора финансовой аренды (лизинга)" && r.mainInfo.Contains(searchString)).ToList();
            //List<PageData> pageData3 = pageData.Where(r => r.type == "Заключение договора финансовой аренды (лизинга)" ).ToList();
            foreach (PageData pd in pageData)
            {
                pd.MainInfo= new MainInfo(pd.mainInfo);
            }
        }
        public string searchString { get; set; }
        public List<PageData> pageData { get; set; }
        public int found { get; set; } = 0;
        public static List<string> GetSearchStrings(string path)
        {
            List<string> list = new List<string>();
            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    list.Add(line);
                }
            }
            return list;
        }
        public static List<Encumbrances> GetFromSite(List<string> list)
        {
            List<Encumbrances> encumbrances = new List<Encumbrances>();
            encumbrances.Write();
            for(int i=0;i<list.Count; i++)
            {
                encumbrances.Add(new Encumbrances(list[i]));
                encumbrances.Write(append:true);
                encumbrances.Clear();
            }
            return encumbrances;
        }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            string s = "";
            foreach(PageData pd in pageData)
            {
                s += string.Format("{1}{0}{2}", separator, searchString, pd.GetStr())+"\n";
            }
            s = s.Trim();
            return s;
        }
        /// <summary>
        /// возвращает пустую строку
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetNullStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}", separator, null, PageData.GetNullStr());
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetTitle(char separator = '\t', string prefix = "")
        {
            prefix += "Encumbrances.";
            return string.Format("{1}{2}{0}{1}{3}",
                separator,
                prefix,
                "searchString",
                PageData.GetTitle());
        }
    }
    public class DocumentsWithHit
    {
        public string guid { get; set; }
        public string name { get; set; }
    }

    public class PageData
    {
        public string mainInfo { get; set; }
        public string number { get; set; }
        public string guid { get; set; }
        public DateTime publishDate { get; set; }
        public bool isAnnuled { get; set; }
        public string type { get; set; }
        public MainInfo MainInfo { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}", separator,
                MainInfo.GetStr(),
                mainInfo.Replace("\t", "~").Replace("\n", "~").Replace("\r", "~"),
                number,
                guid,
                publishDate,
                isAnnuled,
                type,
                MainInfo.GetStr()
                );
        }
        /// <summary>
        /// возвращает пустую строку
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetNullStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}", separator,
                MainInfo.GetNullStr(),
                null,
                null,
                null,
                null,
                null,
                null,
                MainInfo.GetNullStr()
                );
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetTitle(char separator = '\t', string prefix = "")
        {
            prefix += "PageData.";
            return string.Format("{1}{2}{0}{1}{3}{0}{1}{4}{0}{1}{5}{0}{1}{6}{0}{1}{7}{0}{1}{8}{0}{1}{9}",
                separator,
                prefix,
                MainInfo.GetTitle(),
                "mainInfo",
                "number",
                "guid",
                "publishDate",
                "isAnnuled",
                "type",
                MainInfo.GetTitle()
                );
        }
    }
    public class MainInfo
    {
        public MainInfo(string mainInfo)
        {
            string[] rows = mainInfo.Replace("\r\n", "\n").Replace("\t", "").Replace("\n\"", "\"").Replace("\"\n", "\"").Replace("Лизингополучатель: \n", "Лизингополучатель:").Split('\n');
            foreach (string str in rows)
            {
                if(str.Contains("Договор")){ this.Dog = new Dog(str); continue; }
                if (str.Contains("Лизингодатель")){ this.Sender = new Sender(str); continue; }
                if (str.Contains("Лизингополучатель")){ this.Geter = new Geter(str); continue; }
            }

        }
        public Geter Geter{ get; set; }
        public Sender Sender { get; set; }
        public Dog Dog { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}", separator,
                Geter!=null?Geter.GetStr(): Geter.GetNullStr(),
                Sender != null ? Sender.GetStr(): Sender.GetNullStr(),
                Dog != null ? Dog.GetStr(): Dog.GetNullStr());
        }
        /// <summary>
        /// возвращает пустую строку
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetNullStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}", separator, 
                Geter.GetNullStr(),
                Sender.GetNullStr(),
                Dog.GetNullStr());
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetTitle(char separator = '\t', string prefix = "")
        {
            prefix += "MainInfo.";
            return string.Format("{1}{2}{0}{1}{3}{0}{1}{4}",
                separator,
                prefix,
                Geter.GetTitle(),
                Sender.GetTitle(),
                Dog.GetTitle());
        }
    }
    public class Geter
    {
        public Geter(string str)
        {
            if (str == "Лизингодатель: Сведения скрыты в соответствии с требованиями постановления Правительства РФ от 12.01.2018 г. №5\r")
            {
                OGRN = "скрыто";
                INN = "скрыто";
                Name = "скрыто";
                return;
            }
            this.OGRN = str.GetOgrn();
            this.INN = str.GetINN();
            this.Name = str.GetName();
        }
        public string Name { get; set; }
        public string OGRN { get; set; }
        public string INN { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}", separator, Name, OGRN, INN);
        }
        /// <summary>
        /// возвращает пустую строку
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetNullStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}", separator, null, null, null);
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetTitle(char separator = '\t', string prefix = "")
        {
            prefix += "Geter.";
            return string.Format("{1}{2}{0}{1}{3}{0}{1}{4}",
                separator,
                prefix,
                "Name",
                "OGRN",
                "INN");
        }
    }
    public class Sender
    {
        public Sender(string str)
        {
            if(str == "Лизингодатель: Сведения скрыты в соответствии с требованиями постановления Правительства РФ от 12.01.2018 г. №5\r")
            {
                OGRN = "скрыто";
                INN = "скрыто";
                Name = "скрыто";
                return;
            }
            this.OGRN = str.GetOgrn();
            this.INN = str.GetINN();
            this.Name = str.GetName();
        }
        public string Name { get; set; }
        public string OGRN { get; set; }
        public string INN { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}", separator, Name, OGRN, INN);
        }
        /// <summary>
        /// возвращает пустую строку
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetNullStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}", separator, null, null, null);
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetTitle(char separator = '\t', string prefix = "")
        {
            prefix += "Sender.";
            return string.Format("{1}{2}{0}{1}{3}{0}{1}{4}",
                separator,
                prefix,
                "Name",
                "OGRN",
                "INN");
        }
    }
    public class Dog
    {
        public Dog(string str)
        {
            Num = str.GetDogNum();
            Date = str.GetDogDate();
        }
        public string Num { get; set; }
        public DateTime Date { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}", separator, Num, Date);
        }
        /// <summary>
        /// возвращает пустую строку
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetNullStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}", separator, null, null);
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetTitle(char separator = '\t', string prefix = "")
        {
            prefix += "Dog.";
            return string.Format("{1}{2}{0}{1}{3}",
                separator,
                prefix,
                "Num",
                "Date");
        }
    }
    public static class Metods
    {
        public static string GetOgrn(this string str)
        {
            Regex regex = new Regex(@"ОГРН: [0-9]{13}\W|ОГРНИП: [0-9]{15}\W");
            MatchCollection matches = regex.Matches(str);
            string ogrn = matches.FirstOrDefault(r => r.Value != "")?.Value.Replace("ОГРН: ", "").Replace("ОГРНИП: ", "").Replace(",", "").Replace("\r", "");
            return ogrn;
        }
        public static string GetINN(this string str)
        {
            Regex regex = new Regex(@"ИНН: \d{10}\W|ИНН: \d{12}\W");
            MatchCollection matches = regex.Matches(str);
            string inn = matches.FirstOrDefault(r => r.Value != "")?.Value.Replace("ИНН: ", "").Replace(",", "").Replace("\r", "");
            return inn;
        }
        public static string GetName(this string str)
        {
            Regex regex = new Regex("Лизингодатель: [\"\\w\\s]+\\W|Лизингополучатель: [\"\\w\\s]+\\W|");
            MatchCollection matches = regex.Matches(str);
            string name = matches.FirstOrDefault(r => r.Value != "")?.Value.Replace("Лизингодатель: ", "").Replace("Лизингополучатель: ", "").Replace(",", "").Replace("\r", "");
            return name;
        }
        public static string GetDogNum(this string str)
        {
            Regex regex = new Regex("Договор: [\"\\w\\W\\s]+от");
            MatchCollection matches = regex.Matches(str);
            string name = matches.FirstOrDefault(r => r.Value != "").Value.Replace("Договор: ", "").Replace("от", "").Replace(",", "").Replace("\r", "");
            return name;
        }
        public static DateTime GetDogDate(this string str)
        {
            Regex regex = new Regex("от [0-9 .]+\\W?");
            MatchCollection matches = regex.Matches(str);
            string date = matches.FirstOrDefault(r => r.Value != "").Value.Replace("Договор: ", "").Replace("от", "").Replace(",", "").Replace("\r", "");
            return Convert.ToDateTime(date);
        }

        public static void Write(this List<Encumbrances> encumbrances, string name = "Encumbrances.txt", bool append = false, char separator = '\t')
        {
            using (StreamWriter sw = new StreamWriter(name, append, Encoding.UTF8))
            {
                if (!append) { sw.WriteLine(Encumbrances.GetTitle(separator)); }
                foreach (Encumbrances encumbrance in encumbrances)
                {
                    sw.WriteLine(encumbrance.GetStr(separator));
                }
            }
        }
    }
}
