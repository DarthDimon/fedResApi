using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Linq;
using Proxys;

namespace fedResApi2
{
    public class Search
    {
        public Search() { }
        /// <summary>
        /// возвращает класс заполненный с федресурса
        /// </summary>
        /// <param name="inn">ИНН для поиска</param>
        public Search(string inn)
        {
            string strQuery;
        linq2: switch (inn.Length)
            {
                case (10):
                    strQuery = "https://fedresurs.ru/backend/companies?limit=15&offset=0&code=" + inn;
                    break;
                case (12):
                    strQuery = "https://fedresurs.ru/backend/persons?limit=15&offset=0&code=" + inn;
                    break;
                case (9):
                case (11):
                    inn = 0 + inn;
                    goto linq2;
                default:
                    return;
            }
        linq1: HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(strQuery);
            request.UserAgent = "Mozila/4.0 (compatible; MSIE 6.0;Windows NT 5.1; SV1; MyIE2; ";
            request.CookieContainer = new CookieContainer();
            request.Accept = "image/gif, image/x-xbitmap, image/jpeg,image / pjpeg, application / x - shockwave - flash, application / vnd.ms - excel, application / vnd.ms - powerpoint,  application / msword,";
            request.Headers.Add("Accept-Language", "ru");
            request.ContentType = "application/json";
            request.Referer = "fedresurs.ru";
            request.KeepAlive = true;
            request.Timeout = 200000;
            request.Referer = "https://fedresurs.ru/search/entity?code=" + inn;
            HttpWebResponse response = new HttpWebResponse();
            request.Proxy = ProxyClass.GetProxy();
            //{"The remote server returned an error: (400) ."}
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException wEx)
            {
                switch (wEx.Message)
                {
                    case "The operation has timed out.":
                    case "The remote server returned an error: (400) .":
                    case "The remote server returned an error: (404) Not Found.":
                    case "The remote server returned an error: (302) Found.":
                    case "The SSL connection could not be established, see inner exception. The remote certificate is invalid according to the validation procedure.":
                    case "An error occurred while sending the request. Unable to read data from the transport connection: Удаленный хост принудительно разорвал существующее подключение..":
                    case "The remote server returned an error: (400) Page not found.":
                    case "An error occurred while sending the request. Unable to read data from the transport connection: Попытка установить соединение была безуспешной, т.к. от другого компьютера за требуемое время не получен нужный отклик, или было разорвано уже установленное соединение из-за неверного отклика уже подключенного компьютера..":
                    case "The remote server returned an error: (403) Forbidden.":
                    case "The SSL connection could not be established, see inner exception. Authentication failed because the remote party has closed the transport stream.":
                    case "The remote server returned an error: (503) unknown method.":
                    case "The remote server returned an error: (503) Service Unavailable.":
                    case "The remote server returned an error: (500) Internal Server Error.":
                    case "The remote server returned an error: (400) Bad Request."://
                    case "The remote server returned an error: (405) Method Not Allowed.":
                    case "The SSL connection could not be established, see inner exception. The handshake failed due to an unexpected packet format.":
                    case "The remote server returned an error: (302) Redirect.":
                    case "An error occurred while sending the request. The response ended prematurely.":
                    case "Подключение не установлено, т.к. конечный компьютер отверг запрос на подключение. Подключение не установлено, т.к. конечный компьютер отверг запрос на подключение.":
                    case "Попытка установить соединение была безуспешной, т.к. от другого компьютера за требуемое время не получен нужный отклик, или было разорвано уже установленное соединение из-за неверного отклика уже подключенного компьютера. Попытка установить соединение была безуспешной, т.к. от другого компьютера за требуемое время не получен нужный отклик, или было разорвано уже установленное соединение из-за неверного отклика уже подключенного компьютера.":
                        ProxyClass.NexProxy();
                        break;
                    default:
                        var a = 0;
                        ProxyClass.NexProxy();
                        break;
                }
                request.Abort();
                goto linq1;
            }
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(response.GetResponseStream(), Encoding.UTF8);
            JObject jObject = JObject.Parse(doc.Text);
            if (jObject["found"].ToString() == "0") { return; }
            guid = jObject["pageData"][0]["guid"].ToString();
            this.inn = jObject["pageData"][0]["inn"].ToString();
            name = jObject["pageData"][0]["name"].ToString();
            if (inn.Length == 10)
            {
                ogrn = jObject["pageData"][0]["ogrn"].ToString();
                egrulAddress = jObject["pageData"][0]["egrulAddress"].ToString();
                status = jObject["pageData"][0]["status"]?.ToString();
                statusUpdateDate = jObject["pageData"][0]["statusUpdateDate"]?.ToString();
                isActive = Convert.ToBoolean(jObject["pageData"][0]["isActive"]?.ToString());
            }
            request.Abort();
        }
        public string guid { get; set; }
        public string ogrn { get; set; }
        public string inn { get; set; }
        public string name { get; set; }
        public string egrulAddress { get; set; }
        public string status { get; set; }
        public string statusUpdateDate { get; set; }
        public bool isActive { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}", separator, guid, ogrn, inn, name, egrulAddress, status, statusUpdateDate, isActive);
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetTitle(char separator = '\t')
        {
            return string.Format("guid{0}ogrn{0}inn{0}name{0}egrulAddress{0}status{0}statusUpdateDate{0}isActive{0}", separator);
        }
    }
    public static class SearchM
    {
        /// <summary>
        /// добавляет в список из текста
        /// </summary>
        /// <param name="searches">список Search в который производится запись</param>
        /// <param name="path">путь к файлу</param>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static List<Search> AddFromTxt(this List<Search> searches, string path = "searchesList.txt", char separator = '\t')
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string line = sr.ReadLine();
                while ((line = sr.ReadLine()) != null)
                {
                    string[] col = line.Split(separator);
                    Search search = new Search();
                    search.guid = col[0];
                    search.ogrn = col[1];
                    search.inn = col[2];
                    search.name = col[3];
                    search.egrulAddress = col[4];
                    search.status = col[5];
                    search.statusUpdateDate = col[6];
                    search.isActive = Convert.ToBoolean(col[7]);
                    searches.Add(search);
                }
            }
            return searches;
        }
        /// <summary>
        /// добавляет в список с сайта по списку инн
        /// </summary>
        /// <param name="searches">список Search в который производится запись запись</param>
        /// <param name="inns">список ИНН</param>
        /// <param name="writeLengh">по достижению какого числа дозапись в текстовик и очистка списка(0-без записи)</param>
        /// <returns></returns>
        public static List<Search> AddFromSite(this List<Search> searches, List<string> inns, int writeLengh = 0)
        {
            if (inns == null || inns.Count == 0) { return searches; }
            if (writeLengh > 0) { searches.Write(append: false); }
            for (int i = 0; i < inns.Count; i++)
            {
                searches.Add(new Search(inns[i]));
                if ((searches.Count == writeLengh && i % writeLengh == 0 && i > 0) || writeLengh == 1)
                {
                    searches.Write(append: true);
                    searches.Clear();
                }
            }
            if (writeLengh > 0)
            {
                searches = searches.Where(r => r.guid != null).ToList();
                searches.Write(append: true);
                searches.Clear();
                searches = searches.AddFromTxt();
            }
            return searches;
        }
        /// <summary>
        /// записывает в файл
        /// </summary>
        /// <param name="searches">список Search для записи</param>
        /// <param name="name">путь и название файла</param>
        /// <param name="append">true-дозаписать в файл, false-новый файл</param>
        /// <param name="separator">разделитель</param>
        public static void Write(this List<Search> searches, string name = "searchesList.txt", bool append = false, char separator = '\t')
        {
            using (StreamWriter sw = new StreamWriter(name, append, Encoding.UTF8))
            {
                if (!append) { sw.WriteLine(new Search().GetTitle(separator)); }
                foreach (Search search in searches)
                {
                    sw.WriteLine(search.GetStr(separator));
                }
            }
        }
        /// <summary>
        /// получает список инн из файла
        /// </summary>
        /// <param name="path">путь к текстовику с инн</param>
        /// <returns></returns>
        public static List<string> GetListInns(string path)
        {
            List<string> list = new List<string>();
            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    list.Add(line.Replace(".", "").Replace(",", "").Replace(";", "").Trim());
                }
            }
            return list;
        }
        public static List<Search> GetSearches(this List<Person> people, bool persons, bool companies, bool nonresidentcompanies)
        {
            List<Search> searches = new List<Search>();
            foreach (Person person in people)
            {
                searches = searches.Concat(person.GetSearches(persons, companies, nonresidentcompanies)).ToList();
            }
            return searches;
        }
        public static List<Search> GetSearches(this Person person, bool persons, bool companies, bool nonresidentcompanies)
        {
            List<Search> searches;
        linq1: HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://fedresurs.ru/backend/nonresidentcompanies?limit=15&offset=0&name="+person.GetFullNameStr());
            request.UserAgent = "Mozila/4.0 (compatible; MSIE 6.0;Windows NT 5.1; SV1; MyIE2; ";
            request.CookieContainer = new CookieContainer();
            request.Accept = "image/gif, image/x-xbitmap, image/jpeg,image / pjpeg, application / x - shockwave - flash, application / vnd.ms - excel, application / vnd.ms - powerpoint,  application / msword,";
            request.Headers.Add("Accept-Language", "ru");
            request.ContentType = "application/json";
            request.Referer = "fedresurs.ru";
            request.KeepAlive = true;
            request.Timeout = 200000;
            request.Referer = "Referer: https://fedresurs.ru/search/entity?name=" + person.GetFullNameStr();
            HttpWebResponse response = new HttpWebResponse();
            request.Proxy = ProxyClass.GetProxy();
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException wEx)
            {
                switch (wEx.Message)
                {
                    case "The operation has timed out.":
                    case "The remote server returned an error: (400) .":
                    case "The remote server returned an error: (404) Not Found.":
                    case "The remote server returned an error: (302) Found.":
                    case "The SSL connection could not be established, see inner exception. The remote certificate is invalid according to the validation procedure.":
                    case "An error occurred while sending the request. Unable to read data from the transport connection: Удаленный хост принудительно разорвал существующее подключение..":
                    case "The remote server returned an error: (400) Page not found.":
                    case "An error occurred while sending the request. Unable to read data from the transport connection: Попытка установить соединение была безуспешной, т.к. от другого компьютера за требуемое время не получен нужный отклик, или было разорвано уже установленное соединение из-за неверного отклика уже подключенного компьютера..":
                    case "The remote server returned an error: (403) Forbidden.":
                    case "The SSL connection could not be established, see inner exception. Authentication failed because the remote party has closed the transport stream.":
                    case "The remote server returned an error: (503) unknown method.":
                    case "The remote server returned an error: (503) Service Unavailable.":
                    case "The remote server returned an error: (500) Internal Server Error.":
                    case "The remote server returned an error: (400) Bad Request."://
                    case "The remote server returned an error: (405) Method Not Allowed.":
                    case "The SSL connection could not be established, see inner exception. The handshake failed due to an unexpected packet format.":
                    case "The remote server returned an error: (302) Redirect.":
                    case "An error occurred while sending the request. The response ended prematurely.":
                    case "Подключение не установлено, т.к. конечный компьютер отверг запрос на подключение. Подключение не установлено, т.к. конечный компьютер отверг запрос на подключение.":
                    case "Попытка установить соединение была безуспешной, т.к. от другого компьютера за требуемое время не получен нужный отклик, или было разорвано уже установленное соединение из-за неверного отклика уже подключенного компьютера. Попытка установить соединение была безуспешной, т.к. от другого компьютера за требуемое время не получен нужный отклик, или было разорвано уже установленное соединение из-за неверного отклика уже подключенного компьютера.":
                        ProxyClass.NexProxy();
                        break;
                    default:
                        var a = 0;
                        ProxyClass.NexProxy();
                        break;
                }
                request.Abort();
                goto linq1;
            }
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(response.GetResponseStream(), Encoding.UTF8);
            JObject jObject = JObject.Parse(doc.Text);
            if (jObject["found"].ToString() == "0") { return new List<Search>(); }
            searches = jObject["pageData"].ToObject<List<Search>>();
            request.Abort();
            return null;
        }
    }
}
