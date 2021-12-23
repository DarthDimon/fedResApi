using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;
using Proxys;

namespace fedResApi2
{
    public class PageData
    {
        public PageData()
        {

        }
        public string guid { get; set; }
        public string number { get; set; }
        public DateTime datePublish { get; set; }
        public bool isAnnuled { get; set; }
        public bool isLocked { get; set; }
        public string title { get; set; }
        public string publisherName { get; set; }
        public string type { get; set; }
        public string publisherType { get; set; }
        public string bankruptName { get; set; }
        public bool isRefuted { get; set; }
        public string subTitle { get; set; }
        public string inn { get; set; }
        public string participants { get; set; }

        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}{0}{12}{0}{13}{0}{14}",
                separator,
                guid,
                number,
                datePublish,
                isAnnuled,
                isLocked,
                title,
                publisherName,
                type,
                publisherType,
                bankruptName,
                isRefuted,
                subTitle,
                inn,
                participants);
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetTitle(char separator = '\t')
        {
            return string.Format("guid{0}number{0}datePublish{0}isAnnuled{0}isLocked{0}title{0}publisherName{0}type{0}publisherType{0}bankruptName{0}isRefuted{0}subTitle{0}inn{0}participants", separator);
        }
    }
    public static class PageDataM
    {
        /// <summary>
        /// добавляет в список из текста
        /// </summary>
        /// <param name="pageDatas">список PageData в который производится запись</param>
        /// <param name="path">путь к файлу</param>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static List<PageData> AddFromTxt(this List<PageData> pageDatas, string path = "PageDataList.txt", char separator = '\t')
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string line = sr.ReadLine();
                while ((line = sr.ReadLine()) != null)
                {
                    string[] col = line.Split(separator);
                    PageData pageData = new PageData();
                    pageData.guid = col[0];
                    pageData.number = col[1];
                    pageData.datePublish = Convert.ToDateTime(col[2]);
                    pageData.isAnnuled = Convert.ToBoolean(col[3]);
                    pageData.isLocked = Convert.ToBoolean(col[4]);
                    pageData.title = col[5];
                    pageData.publisherName = col[6];
                    pageData.type = col[7];
                    pageData.publisherType = col[8];
                    pageData.bankruptName = col[9];
                    pageData.isRefuted = Convert.ToBoolean(col[10]);
                    pageData.subTitle = col[11];
                    pageData.inn = col[12];
                    pageData.participants= col[13];
                    pageDatas.Add(pageData);
                }
            }
            return pageDatas;
        }
        /// <summary>
        /// добавляет в список с сайта по списку инн
        /// </summary>
        /// <param name="pageDatas">список PageData в который производится запись</param>
        /// <param name="inns">список ИНН</param>
        /// <returns></returns>
        public static List<PageData> AddFromSite(this List<PageData> pageDatas, List<Search> searches, int writeLengh = 0)
        {
            if (searches == null || searches.Count == 0) { return pageDatas; }
            if (writeLengh > 0) { pageDatas.Write(append: false); }
            for (int i = 0; i < searches.Count; i++)
            {
                if(searches[i].guid==""|| searches[i].guid == null) { continue; }
                pageDatas = pageDatas.Concat(GetListFromSite(searches[i])).ToList();
                if ((writeLengh > 0 && i % writeLengh == 0 && i > 0) || writeLengh==1)
                {
                    pageDatas.Write(append: true);
                    pageDatas.Clear();
                }
            }
            if (writeLengh > 0)
            {
                pageDatas.Write(append: true);
                pageDatas.Clear();
                pageDatas = pageDatas.AddFromTxt();
            }
            return pageDatas;
        }
        /// <summary>
        /// записывает в файл
        /// </summary>
        /// <param name="pageDatas">список PageData для записи</param>
        /// <param name="name">путь и название файла</param>
        /// <param name="append">true-дозаписать в файл, false-новый файл</param>
        /// <param name="separator">разделитель</param>
        public static void Write(this List<PageData> pageDatas, string name = "PageDataList.txt", bool append = false, char separator = '\t')
        {
            using (StreamWriter sw = new StreamWriter(name, append, Encoding.UTF8))
            {
                if (!append) { sw.WriteLine(new PageData().GetTitle(separator)); }
                foreach (PageData pageData in pageDatas)
                {
                    sw.WriteLine(pageData.GetStr(separator));
                }
            }
        }
        public static List<PageData> GetListFromSite(Search search)
        {
            int limit = 50,
                found = 0,
                offset = 0,
                deltDays = 90;// сколько дней прибавлять к начальной дате
            bool startDofLiz = true;//
            DateTime startDate = new DateTime(2016, 01, 01), endDate = startDate.AddDays(deltDays);
            bool useDate = false;
            List<PageData> pageDatas = new List<PageData>();
            do
            {
                do
                {
                    string strQuery, referer;
                    switch (search.inn.Length)
                    {
                        case (10):
                            strQuery = "https://fedresurs.ru/backend/companies/" + search.guid + "/publications?" +
                                "&limit=" + limit +
                                "&offset=" + offset +
                                "&searchCompanyEfrsb=true" +
                                "&searchAmReport=true" +
                                "&searchFirmBankruptMessage=true" +
                                "&searchFirmBankruptMessageWithoutLegalCase=false" +
                                "&searchSfactsMessage=true" +
                                "&searchSroAmMessage=true" +
                                "&searchTradeOrgMessage=true";
                            referer = "https://fedresurs.ru/company/b99c2ded-1a6c-4239-b728-804461ad074b" + search.guid;
                            break;
                        case (12):
                            strQuery = "https://fedresurs.ru/backend/persons/" + search.guid + "/publications?" +
                                "limit=" + limit +
                                "&offset=" + offset +
                                "&searchPersonEfrsbMessage=true&searchAmReport=true" +
                                "&searchPersonBankruptMessage=true" +
                                "&searchMessageOnlyWithoutLegalCase=false" +
                                "&searchSfactsMessage=true" +
                                "&searchArbitrManagerMessage=true" +
                                "&searchTradeOrgMessage=true";
                            referer = "https://fedresurs.ru/person/b99c2ded-1a6c-4239-b728-804461ad074b" + search.guid;
                            break;
                        default:
                            return pageDatas;
                    }
                    //if (startDofLiz) { strQuery += "&sfactMessageType=FinancialLeaseContract&sfactsMessageTypeGroupId=3"; }// только лизинг
                    if (useDate)
                    {
                        endDate = startDate.AddDays(deltDays);
                        strQuery += string.Format("&startDate=" + GetDateStr(startDate) + "T00:00:00.000Z&endDate=" + GetDateStr(endDate) + "T00:00:00.000Z");
                    }
                linq1: HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(strQuery);
                    request.UserAgent = "Mozila/4.0 (compatible; MSIE 6.0;Windows NT 5.1; SV1; MyIE2; ";
                    request.CookieContainer = new CookieContainer();
                    request.Accept = "image/gif, image/x-xbitmap, image/jpeg,image / pjpeg, application / x - shockwave - flash, application / vnd.ms - excel, application / vnd.ms - powerpoint,  application / msword,";
                    request.Headers.Add("Accept-Language", "ru");
                    request.ContentType = "application/json";
                    request.Referer = "fedresurs.ru";
                    request.KeepAlive = true;
                    request.Referer = referer;
                    request.Proxy = ProxyClass.GetProxy();
                    HttpWebResponse response;
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
                    if (found == 0) { found = Convert.ToInt32(jObject["found"]); }
                    if (found == 0) { break; }
                    if (found > 500 && deltDays > 0)
                    {
                        if (useDate)
                        {
                            deltDays /= 2; // меняем кол-во дней если слишком много сообщений
                            break;
                        }
                        else
                        {
                            useDate = true; // включаем поиск по датам если слишком много сообщений
                            break;
                        }
                    }
                    if (found > 500 && deltDays == 0)
                    {
                        var c = 0;
                    }
                    List<PageData> pageData = jObject["pageData"].ToObject<List<PageData>>();
                    if (pageData.Count > 0) { pageDatas = pageDatas.Concat(jObject["pageData"].ToObject<List<PageData>>()).ToList(); }
                    offset += limit;
                } while (offset < found);
                if (found <= 500 || deltDays<1)
                {
                    startDate = endDate.AddDays(1);
                    deltDays = 90;
                }
                found = 0;
                offset = 0;
            }
            while (startDate <= DateTime.Now && useDate);
            pageDatas = pageDatas.Distinct().ToList();
            foreach (PageData pageData in pageDatas)
            {
                pageData.inn = search.inn;
            }
            return pageDatas;

            string GetDateStr(DateTime dateTime)
            {
                return string.Format("{0}-{1}-{2}", dateTime.Year, dateTime.Month < 10 ? "0" + dateTime.Month : dateTime.Month.ToString(), dateTime.Day < 10 ? "0" + dateTime.Day : dateTime.Day.ToString());
            }
        }
    }
}
