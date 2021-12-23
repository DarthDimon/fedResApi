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
    public class MessageFedRes
    {
        public MessageFedRes() { }
        public MessageFedRes(PageData pageData)
        {
            string queryStr, refStr;
            switch (pageData.type)
            {
                case "SfactMessage":
                    queryStr = "https://fedresurs.ru/backend/sfactmessages/" + pageData.guid;
                    refStr = "https://fedresurs.ru/sfactmessage/" + pageData.guid;
                    break;
                case "BankruptcyMessage":
                    queryStr = "https://fedresurs.ru/backend/bankruptcymessages/" + pageData.guid;
                    refStr = "https://fedresurs.ru/bankruptmessage/" + pageData.guid;
                    break;
                default:
                    var a = 0;
                    return;
            }
        linq1: HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(queryStr);
            request.UserAgent = "Mozila/4.0 (compatible; MSIE 6.0;Windows NT 5.1; SV1; MyIE2; ";
            request.CookieContainer = new CookieContainer();
            request.Accept = "image/gif, image/x-xbitmap, image/jpeg,image / pjpeg, application / x - shockwave - flash, application / vnd.ms - excel, application / vnd.ms - powerpoint,  application / msword,";
            request.Headers.Add("Accept-Language", "ru");
            request.ContentType = "application/json";
            request.Referer = "fedresurs.ru";
            request.Proxy = ProxyClass.GetProxy();
            request.KeepAlive = true;
            request.Referer = refStr;
            HttpWebResponse response = null;
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
                    case "The remote server returned an error: (400) Bad Request.":
                        guid = pageData.guid;
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
            messageType = jObject["messageType"].ToString();
            var c = jObject["content"];
            content = jObject["content"].ToObject<Content>();
            docs = jObject["docs"].ToObject<List<Doc>>();
            publisher = jObject["publisher"].ToObject<Publisher>();
            guid = jObject["guid"].ToString();
            number = jObject["number"].ToString();
            typeName = jObject["typeName"].ToString();
            datePublish = Convert.ToDateTime(jObject["datePublish"]);
            if (jObject["bankrupt"] != null) { bankrupt = jObject["bankrupt"].ToObject<Bankrupt>(); }
        }
        public MessageFedRes(string guidMsg, string typeMsg)
        {
            string queryStr;
            switch (typeMsg)
            {

            }
        linq1: HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://fedresurs.ru/backend/bankruptcymessages/" + guidMsg);
            request.UserAgent = "Mozila/4.0 (compatible; MSIE 6.0;Windows NT 5.1; SV1; MyIE2; ";
            request.CookieContainer = new CookieContainer();
            request.Accept = "image/gif, image/x-xbitmap, image/jpeg,image / pjpeg, application / x - shockwave - flash, application / vnd.ms - excel, application / vnd.ms - powerpoint,  application / msword,";
            request.Headers.Add("Accept-Language", "ru");
            request.ContentType = "application/json";
            request.Referer = "fedresurs.ru";
            request.Proxy = ProxyClass.GetProxy();
            request.KeepAlive = true;
            request.Referer = "https://fedresurs.ru/bankruptmessage/" + guidMsg;
            HttpWebResponse response = null;
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
                    case "The remote server returned an error: (400) Bad Request.":
                        guid = guidMsg;
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
            messageType = jObject["messageType"].ToString();
            bankrupt = jObject["bankrupt"].ToObject<Bankrupt>();
            content = jObject["content"].ToObject<Content>();
            docs = jObject["docs"].ToObject<List<Doc>>();
            publisher = jObject["publisher"].ToObject<Publisher>();
            guid = jObject["guid"].ToString();
            number = jObject["number"].ToString();
            typeName = jObject["typeName"].ToString();
            datePublish = Convert.ToDateTime(jObject["datePublish"]);
        }

        public string messageType { get; set; }
        public Bankrupt bankrupt { get; set; }
        public Content content { get; set; }
        public List<Doc> docs { get; set; }
        public Publisher publisher { get; set; }
        public string guid { get; set; }
        public DateTime datePublish { get; set; }
        public string number { get; set; }
        public string typeName { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}",
                separator,
                messageType,
                bankrupt != null ? bankrupt.GetStr() : Bankrupt.GetNullStr(),
                content != null ? content.GetStr() : Content.GetNullStr(),
                //docs.GetStr(), 
                publisher != null ? publisher.GetStr() : Publisher.GetNullStr(),
                guid,
                datePublish,
                number,
                typeName);
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetTitle(char separator = '\t', string prefix = "")
        {
            prefix += "Bankrupt.";
            return string.Format("{1}{2}{0}{1}{3}{0}{1}{4}{0}{1}{5}{0}{1}{6}{0}{1}{7}{0}{1}{8}{0}{1}{9}",
                separator,
                prefix,
                "messageType",
                Bankrupt.GetTitle(),
                Content.GetTitle(),
                //docs.GetStr(), 
                Publisher.GetTitle(),
                "guid",
                "datePublish",
                "number",
                "typeName");
        }
    }
    public class Bankrupt
    {
        public string type { get; set; }
        public string address { get; set; }
        public string ogrn { get; set; }
        public string name { get; set; }
        public string inn { get; set; }
        public string legalCaseNumber { get; set; }
        public Category category { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}",
                separator,
                type,
                address,
                ogrn,
                name,
                inn,
                legalCaseNumber,
                category != null ? category.GetStr() : Category.GetNullStr());
        }
        /// <summary>
        /// возвращает пустую строку
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetNullStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}",
                separator,
                null,
                null,
                null,
                null,
                null,
                null,
                Category.GetNullStr());
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <param name="prefix">префикс который ставится перед каждым названием поля</param>
        /// <returns></returns>
        public static string GetTitle(char separator = '\t', string prefix = "")
        {
            prefix += "Bankrupt.";
            return string.Format("{1}{2}{0}{1}{3}{0}{1}{4}{0}{1}{5}{0}{1}{6}{0}{1}{7}{0}{1}{8}",
                separator,
                prefix,
                "type",
                "address",
                "ogrn",
                "name",
                "inn",
                "legalCaseNumber",
                Category.GetTitle());
        }
    }
    public class Fio
    {
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}", separator, lastName, firstName, middleName);
        }
        /// <summary>
        /// возвращает пустую строку
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetNullStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}", separator, null, null, null, null);
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetTitle(char separator = '\t', string prefix = "")
        {
            prefix += "Fio.";
            return string.Format("{1}lastName{0}{1}firstName{0}{1}middleName", separator, prefix);
        }
    }
    public class Sro
    {
        public string name { get; set; }
        public string ogrn { get; set; }
        public string inn { get; set; }
        public string address { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}", separator, name, ogrn, inn, address);
        }
        /// <summary>
        /// возвращает пустую строку
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetNullStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}", separator, null, null, null, null);
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetTitle(char separator = '\t', string prefix = "")
        {
            prefix += "Sro.";
            return string.Format("{1}name{0}{1}ogrn{0}{1}inn{0}{1}address", separator, prefix);
        }
    }
    public class Publisher
    {
        public Fio fio { get; set; }
        public string inn { get; set; }
        public string snils { get; set; }
        public string correspondenceAddress { get; set; }
        public Sro sro { get; set; }
        public string type { get; set; }
        public SroInfo sroInfo { get; set; }
        public string name { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}", separator,
                fio != null ? fio.GetStr() : Fio.GetNullStr(),
                inn,
                snils,
                correspondenceAddress,
                sro != null ? sro.GetStr() : Sro.GetNullStr(),
                type,
                sroInfo != null ? sroInfo.GetStr() : SroInfo.GetNullStr(),
                name);
        }
        /// <summary>
        /// возвращает пустую строку
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetNullStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}", separator,
                Fio.GetNullStr(),
                null,
                null,
                null,
                Sro.GetNullStr(),
                null,
                SroInfo.GetNullStr(),
                null);
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetTitle(char separator = '\t', string prefix = "")
        {
            prefix += "Publisher.";
            return string.Format("{1}{2}" +
                "{0}{1}{3}" +
                "{0}{1}{4}" +
                "{0}{1}{5}" +
                "{0}{1}{6}" +
                "{0}{1}{7}" +
                "{0}{1}{8}" +
                "{0}{1}{9}", separator, prefix, Fio.GetTitle(), "inn", "snils", "correspondenceAddress", Sro.GetTitle(), "type", SroInfo.GetTitle(), "name");
        }
    }
    public class MessageContent
    {
        public DecisionType decisionType { get; set; }
        public CourtDecree courtDecree { get; set; }
        public DateTime nextCourtSessionDate { get; set; }
        public string arbitrManagerType { get; set; }
        public bool arbitrManagerTypeSpecified { get; set; }
        public string text { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}", separator,
                decisionType != null ? decisionType.GetStr() : DecisionType.GetNullStr(),
                courtDecree != null ? courtDecree.GetStr() : CourtDecree.GetNullStr(),
                nextCourtSessionDate,
                arbitrManagerType,
                arbitrManagerTypeSpecified,
                text.Replace("\t", "~").Replace("\r", "~").Replace("\n", "~"));
        }
        /// <summary>
        /// возвращает пустую строку
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetNullStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}", separator,
                DecisionType.GetNullStr(),
                CourtDecree.GetNullStr(),
                null,
                null,
                null,
                null);
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetTitle(char separator = '\t', string prefix = "")
        {
            prefix += "MessageContent.";
            return string.Format("{1}{2}{0}{1}{3}{0}{1}{4}{0}{1}{5}{0}{1}{6}{0}{1}{7}", separator, prefix,
                DecisionType.GetTitle(),
                CourtDecree.GetTitle(),
                "nextCourtSessionDate",
                "arbitrManagerType",
                "arbitrManagerTypeSpecified",
                "text");
        }

    }
    public class CourtDecree
    {
        public int courtId { get; set; }
        public string courtName { get; set; }
        public string fileNumber { get; set; }
        public DateTime decisionDate { get; set; }
        public bool decisionDateSpecified { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}", separator,
                courtId,
                courtName,
                fileNumber,
                decisionDate,
                decisionDateSpecified);
        }
        /// <summary>
        /// возвращает пустую строку
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetNullStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}", separator,
                null,
                null,
                null,
                null,
                null);
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetTitle(char separator = '\t', string prefix = "")
        {
            prefix += "CourtDecree.";
            return string.Format("{1}{2}{0}{1}{3}{0}{1}{4}{0}{1}{5}{0}{1}{6}", separator, prefix,
                "courtId",
                "courtName",
                "fileNumber",
                "decisionDate",
                "decisionDateSpecified");
        }
    }
    public class DecisionType
    {
        public string name { get; set; }
        public int id { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}", separator,
                name,
                id);
        }
        /// <summary>
        /// возвращает пустую строку
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetNullStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}", separator,
                null,
                null);
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetTitle(char separator = '\t', string prefix = "")
        {
            prefix += "DecisionType.";
            return string.Format("{1}{2}{0}{1}{3}", separator, prefix, "name", "id");
        }
    }
    public class MessageInfo
    {
        public MessageContent messageContent { get; set; }
        public string messageType { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}", separator,
                messageContent!=null?messageContent.GetStr():MessageContent.GetNullStr(), messageType);
        }
        /// <summary>
        /// возвращает пустую строку
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetNullStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}", separator, MessageContent.GetNullStr(), null);
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetTitle(char separator = '\t', string prefix = "")
        {
            prefix += "";
            return string.Format("{1}{2}{0}{1}{3}",
                separator, prefix, MessageContent.GetTitle(), "messageType");
        }
    }
    public class Category
    {
        public string code { get; set; }
        public string description { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}", separator, code, description);
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
            prefix += "Category.";
            return string.Format("{1}code{0}{1}description", separator, prefix);
        }
    }
    public class FileInfoList
    {
        public string name { get; set; }
        public string hash { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}", separator, name, hash);
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
            prefix += "FileInfoList.";
            return string.Format("{1}name{0}{1}hash", separator, prefix);
        }
    }
    public class Creditor
    {
        public string shortName { get; set; }
        public string ogrn { get; set; }
        public string inn { get; set; }
        public string fio { get; set; }
        public string ogrnip { get; set; }
    }
    public class Content
    {
        public string caseNumber { get; set; }
        public Publisher publisher { get; set; }
        public MessageInfo messageInfo { get; set; }
        public Bankrupt bankrupt { get; set; }
        public List<FileInfoList> fileInfoList { get; set; }
        public bool fileInfoListSpecified { get; set; }
        public string messageType { get; set; }
        public string text { get; set; }
        public List<Creditor> creditors { get; set; }
        public Debtor debtor { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}",
                separator,
                caseNumber,
                publisher != null ? publisher.GetStr() : Publisher.GetNullStr(),
                messageInfo != null ? messageInfo.GetStr() : MessageInfo.GetNullStr(),
                bankrupt != null ? bankrupt.GetStr() : Bankrupt.GetNullStr(),
                //bankrupt.GetStr(),
                fileInfoListSpecified,
                messageType,
                text?.Replace("\t", "~t~").Replace("\r", "~").Replace("\n", "~"),
                debtor != null ? debtor.GetStr() : Debtor.GetNullStr());
        }
        /// <summary>
        /// возвращает пустую строку
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetNullStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}", separator, null, Publisher.GetNullStr(), MessageInfo.GetNullStr(), Bankrupt.GetNullStr(), null, null, null, Debtor.GetNullStr());
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetTitle(char separator = '\t', string prefix = "")
        {
            prefix += "Content.";
            return string.Format("{1}{2}{0}{1}{3}{0}{1}{4}{0}{1}{5}{0}{1}{6}{0}{1}{7}{0}{1}{8}{0}{1}{9}",
                separator,
                prefix,
                "caseNumber",
                Publisher.GetTitle(),
                MessageInfo.GetTitle(),
                Bankrupt.GetTitle(),
                //bankrupt.GetStr(),
                "fileInfoListSpecified",
                "messageType",
                "text",
                Debtor.GetTitle());
        }
    }
    public class Debtor
    {
        public string shortName { get; set; }
        public string ogrn { get; set; }
        public string inn { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}", separator, shortName?.Replace("\t", "~").Replace("\r", "~").Replace("\n", "~"), ogrn, inn);
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
            prefix += "Debtor.";
            return string.Format("{1}{2}{0}{1}{3}{0}{1}{4}",
                separator,
                prefix,
                "shortName",
                "ogrn",
                "inn");
        }
    } //
    public class Doc
    {
        public string guid { get; set; }
        public string name { get; set; }
        public bool isDangerous { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}", separator, guid, name, isDangerous);
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public  static string GetTitle(char separator = '\t', string prefix = "")
        {
            prefix += "Doc.";
            return string.Format("{1}guid{0}{1}name{0}{1}isDangerous", separator, prefix);
        }
    }
    public class SroInfo
    {
        public string name { get; set; }
        public string inn { get; set; }
        public string ogrn { get; set; }
        public string address { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}", separator, name, ogrn, inn, address);
        }
        /// <summary>
        /// возвращает пустую строку
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetNullStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}", separator, null, null, null, null);
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetTitle(char separator = '\t', string prefix = "")
        {
            prefix += "SroInfo.";
            return string.Format("{1}{2}{0}{1}{3}{0}{1}{4}{0}{1}{5}",
                separator,
                prefix,
                "name",
                "ogrn",
                "inn",
                "address");
        }
    } //



    public static class MessageM
    {

        /// <summary>
        /// добавляет в список с сайта по списку инн
        /// </summary>
        /// <param name="messages">список MessageFedRes в который производится запись</param>
        /// <param name="guids">список ИНН</param>
        /// <returns></returns>
        public static List<MessageFedRes> AddFromSite(this List<MessageFedRes> messages, List<string> guids, int writeLengh = 0)
        {
            if (guids == null || guids.Count == 0) { return messages; }
            if (writeLengh > 0) { messages.Write(append: false); }
            for (int i = 0; i < guids.Count; i++)
            {
                messages.Add(new MessageFedRes(guids[i], guids[i]));
                if (writeLengh > 0 && i % writeLengh == 0 && i > 0)
                {
                    messages.Write(append: true);
                    messages.Clear();
                }
            }
            if (writeLengh > 0)
            {
                messages.Write(append: true);
                messages.Clear();
            }
            else { messages.Write(append: false); }
            return messages;
        }
        public static List<MessageFedRes> AddFromSite(this List<MessageFedRes> messages, List<PageData> pageDatas, int writeLengh = 0)
        {
            if (pageDatas == null || pageDatas.Count == 0) { return messages; }
            if (writeLengh > 0) { messages.Write(append: false); }
            List<PageData> guids = pageDatas.Where(r => r.type != "AmReport").ToList();
            //messages = messages.AddFromSite(guids, writeLengh); 
            for (int i = 0; i < guids.Count; i++)
            {
                messages.Add(new MessageFedRes(guids[i]));
                if (writeLengh > 0 && i % writeLengh == 0 && i > 0)
                {
                    messages.Write(append: true);
                    messages.Clear();
                }
            }
            if (writeLengh > 0)
            {
                messages.Write(append: true);
                messages.Clear();
            }
            else 
            {
                messages.Write(append: false); 
            }
            return messages;
        }
        public static void Write(this List<MessageFedRes> messages, string name = "MessageList.txt", bool append = false, char separator = '\t')
        {
            using (StreamWriter sw = new StreamWriter(name, append, Encoding.UTF8))
            {
                if (!append) { sw.WriteLine(MessageFedRes.GetTitle(separator)); }
                foreach (MessageFedRes message in messages)
                {
                    sw.WriteLine(message.GetStr(separator));
                }
            }
        }
    }
}
