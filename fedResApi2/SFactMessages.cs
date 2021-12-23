using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;
using Proxys;

namespace Sfactmessages
{
    public class SFactMessages
    {
        public SFactMessages(string guid)
        {
        linq1: HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://fedresurs.ru/backend/sfactmessages/" + guid);
            request.UserAgent = "Mozila/4.0 (compatible; MSIE 6.0;Windows NT 5.1; SV1; MyIE2; ";
            request.CookieContainer = new CookieContainer();
            request.Accept = "image/gif, image/x-xbitmap, image/jpeg,image / pjpeg, application / x - shockwave - flash, application / vnd.ms - excel, application / vnd.ms - powerpoint,  application / msword,";
            request.Headers.Add("Accept-Language", "ru");
            request.ContentType = "application/json";
            request.Referer = "fedresurs.ru";
            request.Proxy = ProxyClass.GetProxy();
            request.KeepAlive = true;
            request.Referer = "https://fedresurs.ru/sfactmessage/" + guid;
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
                        this.guid = guid;
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
            if (jObject["messageType"] != null) { this.messageType = jObject["messageType"].ToString(); }
            if (jObject["isPublisherInSanctionList"] != null) { this.isPublisherInSanctionList = jObject["isPublisherInSanctionList"].ToObject<bool>(); }
            if (jObject["isParticipantInSanctionList"] != null) { this.isParticipantInSanctionList = jObject["isParticipantInSanctionList"].ToObject<bool>(); }
            if (jObject["content"] != null) { this.content = jObject["content"].ToObject<Content>(); }
            if (jObject["publisher"] != null) { this.publisher = jObject["publisher"].ToObject<Publisher>(); }
            if (jObject["guid"] != null) { this.guid = jObject["guid"].ToObject<string>(); }
            if (jObject["datePublish"] != null) { this.datePublish = jObject["datePublish"].ToObject<DateTime>(); }
            if (jObject["number"] != null) { this.number = jObject["number"].ToObject<string>(); }
            if (jObject["typeName"] != null) { this.typeName = jObject["typeName"].ToObject<string>(); }
        }

        public string messageType { get; set; }
        public bool isPublisherInSanctionList { get; set; }
        public bool isParticipantInSanctionList { get; set; }
        public Content content { get; set; }
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
            string res = "";

            if (content != null && content.lessorsCompanies?.Count > 0 && content.lesseesCompanies?.Count > 0)
            {
                for (int lessorsCompaniesNum = 0; lessorsCompaniesNum < content.lessorsCompanies.Count; lessorsCompaniesNum++)
                {
                    for (int lesseesCompaniesNum = 0; lesseesCompaniesNum < content.lesseesCompanies.Count; lesseesCompaniesNum++)
                    {
                        res += getStr(lessorsCompaniesNum: lessorsCompaniesNum, lesseesCompaniesNum: lesseesCompaniesNum);
                    }
                }
            }
            else
            {
                if (content == null || (content.lessorsCompanies?.Count == 0 && content.lesseesCompanies?.Count == 0) || (content.lessorsCompanies == null && content.lesseesCompanies == null))
                {
                    res += getStr();
                }
                else
                {
                    if (content.lessorsCompanies?.Count > 0)
                    {
                        for (int lessorsCompaniesNum = 0; lessorsCompaniesNum < content.lessorsCompanies.Count; lessorsCompaniesNum++)
                        {
                            res += getStr(lessorsCompaniesNum: lessorsCompaniesNum, lesseesCompaniesNum: 0);
                        }
                    }
                    else
                    {
                        for (int lesseesCompaniesNum = 0; lesseesCompaniesNum < content.lesseesCompanies.Count; lesseesCompaniesNum++)
                        {
                            res += getStr(lessorsCompaniesNum: 0, lesseesCompaniesNum: lesseesCompaniesNum);
                        }
                    }
                }
            }
            res = res.Substring(0, res.Length - 1);
            if (res == "") { var a = 0; }
            return res;
            string getStr(int lessorsCompaniesNum = 0, int lesseesCompaniesNum = 0)
            {
                return res = res += string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}", separator,
                messageType,
                isPublisherInSanctionList,
                isParticipantInSanctionList,
                content != null ? content.GetStr(lessorsCompaniesNum, lesseesCompaniesNum) : Content.GetNullStr(),
                publisher != null ? publisher.GetStr() : Publisher.GetNullStr(),
                guid,
                datePublish,
                number,
                typeName
                ) + "\n";
            }
        }
        /// <summary>
        /// возвращает пустую строку
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetNullStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}", separator,
                null,
                null,
                null,
                Content.GetNullStr(),
                Publisher.GetNullStr(),
                null,
                null,
                null,
                null
                );
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetTitle(char separator = '\t', string prefix = "")
        {
            prefix += "SFactMessages.";
            return string.Format("{1}{2}{0}{1}{3}{0}{1}{4}{0}{1}{5}{0}{1}{6}{0}{1}{7}{0}{1}{8}{0}{1}{9}{0}{1}{10}",
                separator,
                prefix,
                "messageType",
                "isPublisherInSanctionList",
                "isParticipantInSanctionList",
                Content.GetTitle(prefix: "SFactMessages."),
                Publisher.GetTitle(prefix: "SFactMessages."),
                "guid",
                "datePublish",
                "number",
                "typeName"
                );
        }
    }
    public class Subject
    {
        public string guid { get; set; }
        public string classifierCode { get; set; }
        public string classifierName { get; set; }
        public string description { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '~')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}", separator, guid, classifierCode, classifierName, description?.Replace("\t", "").Replace("\r", "").Replace("\n", "").Trim());
        }
        /// <summary>
        /// возвращает пустую строку
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetNullStr(char separator = '~')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}", separator, null, null, null, null);
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetTitle(char separator = '~', string prefix = "")
        {
            prefix += "Subject.";
            return string.Format("{1}{2}{0}{1}{3}{0}{1}{4}{0}{1}{5}",
                separator,
                prefix,
                "guid",
                "classifierCode",
                "classifierName",
                "guid");
        }
    } //Предметы финансовой аренды
    public class LessorsCompany
    {
        public string fullName { get; set; }
        public string inn { get; set; }
        public string ogrn { get; set; }
        public string type { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}", separator, fullName?.Replace("\t", "").Replace("\r", "").Replace("\n", "").Trim(), inn, ogrn, type);
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
            prefix += "LessorsCompany.";
            return string.Format("{1}{2}{0}{1}{3}{0}{1}{4}{0}{1}{5}",
                separator,
                prefix,
                "fullName",
                "inn",
                "ogrn",
                "type");
        }
    } //Лизингодатели
    public class LesseesCompany
    {
        public string fullName { get; set; }
        public string inn { get; set; }
        public string ogrn { get; set; }
        public string type { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}", separator, fullName?.Replace("\t", "").Replace("\r", "").Replace("\n", "").Trim(), inn, ogrn, type);
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
            prefix += "LesseesCompany.";
            return string.Format("{1}{2}{0}{1}{3}{0}{1}{4}{0}{1}{5}",
                separator,
                prefix,
                "fullName",
                "inn",
                "ogrn",
                "type");
        }
    } //Лизингополучатели
    public class PublisherInfo
    {
        public string type { get; set; }
        public string fullName { get; set; }
        public string inn { get; set; }
        public string ogrn { get; set; }
        public string egrulAddress { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}", separator, type, fullName?.Replace("\t", "").Replace("\r", "").Replace("\n", "").Trim(), inn, ogrn, egrulAddress);
        }
        /// <summary>
        /// возвращает пустую строку
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetNullStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}", separator, null, null, null, null, null);
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetTitle(char separator = '\t', string prefix = "")
        {
            prefix += "PublisherInfo.";
            return string.Format("{1}{2}{0}{1}{3}{0}{1}{4}{0}{1}{5}{0}{1}{6}",
                separator,
                prefix,
                "type",
                "fullName",
                "inn",
                "ogrn",
                "egrulAddress");
        }
    }
    public class Content
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public bool isSubleaseContract { get; set; }
        public string contractNumber { get; set; }
        public DateTime contractDate { get; set; }
        public string text { get; set; }
        public PublisherInfo publisherInfo { get; set; }
        public List<LessorsCompany> lessorsCompanies { get; set; }
        public List<LesseesCompany> lesseesCompanies { get; set; }
        public List<Subject> subjects { get; set; }


        public List<PledgeeCompany> pledgeeCompanies { get; set; }
        public List<MortgagorPerson> mortgagorPersons { get; set; }
        //public List<object> messageDocList { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(int lessorsCompaniesNum, int lesseesCompaniesNum, char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}{0}{12}", separator,
                startDate,
                endDate,
                isSubleaseContract,
                contractNumber,
                contractDate,
                text?.Replace("\t", "").Replace("\r", "").Replace("\n", "").Trim(),
                publisherInfo != null ? publisherInfo.GetStr() : PublisherInfo.GetNullStr(),
                lessorsCompanies != null && lessorsCompanies.Count > 0 ? lessorsCompanies[lessorsCompaniesNum].GetStr() : LessorsCompany.GetNullStr(),
                lesseesCompanies != null && lesseesCompanies.Count > 0 ? lesseesCompanies[lesseesCompaniesNum].GetStr() : LesseesCompany.GetNullStr(),
                subjects != null ? subjects.GetStr() : Subject.GetNullStr()
                , pledgeeCompanies != null ? pledgeeCompanies.GetStr() : null
                , mortgagorPersons != null ? mortgagorPersons.GetStr() : null
                );
        }
        /// <summary>
        /// возвращает пустую строку
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetNullStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}{0}{12}", separator,
                null,
                null,
                null,
                null,
                null,
                null,
                PublisherInfo.GetNullStr(),
                LessorsCompany.GetNullStr(),
                LesseesCompany.GetNullStr(),
                Subject.GetNullStr(),
                null,
                null
                );
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetTitle(char separator = '\t', string prefix = "")
        {
            prefix += "Content.";
            return string.Format("{1}{2}{0}{1}{3}{0}{1}{4}{0}{1}{5}{0}{1}{6}{0}{1}{7}{0}{1}{8}{0}{1}{9}{0}{1}{10}{0}{1}{11}{0}{1}{12}{0}{1}{13}",
                separator,
                prefix,
                "startDate",
                "endDate",
                "isSubleaseContract",
                "contractNumber",
                "contractDate",
                "text",
                PublisherInfo.GetTitle(prefix: prefix),
                LessorsCompany.GetTitle(prefix: prefix),
                LesseesCompany.GetTitle(prefix: prefix),
                Subject.GetTitle(prefix: prefix),
                new List<PledgeeCompany>().GetTitle(prefix: prefix),
                new List<MortgagorPerson>().GetTitle(prefix: prefix)
                );
        }
    }

    public class Publisher
    {
        public string ogrn { get; set; }
        public string inn { get; set; }
        public string address { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}", separator, ogrn, inn, address, type, name?.Replace("\t", "").Replace("\r", "").Replace("\n", "").Trim());
        }
        /// <summary>
        /// возвращает пустую строку
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetNullStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}", separator, null, null, null, null, null);
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetTitle(char separator = '\t', string prefix = "")
        {
            prefix += "Publisher.";
            return string.Format("{1}{2}{0}{1}{3}{0}{1}{4}{0}{1}{5}{0}{1}{6}",
                separator,
                prefix,
                "ogrn",
                "inn",
                "address",
                "type",
                "name");
        }
    }
    public class PledgeeCompany
    {
        public string fullName { get; set; }
        public string shortName { get; set; }
        public string address { get; set; }
        public string inn { get; set; }
        public string ogrn { get; set; }
        public string email { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}", separator,
                fullName?.Replace("\t", "").Replace("\r", "").Replace("\n", "").Trim(),
                shortName?.Replace("\t", "").Replace("\r", "").Replace("\n", "").Trim(),
                address?.Replace("\t", "").Replace("\r", "").Replace("\n", "").Trim(),
                inn?.Replace("\t", "").Replace("\r", "").Replace("\n", "").Trim(),
                ogrn?.Replace("\t", "").Replace("\r", "").Replace("\n", "").Trim(),
                email?.Replace("\t", "").Replace("\r", "").Replace("\n", "").Trim()
                );
        }
        /// <summary>
        /// возвращает пустую строку
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetNullStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}", separator, null, null, null, null, null, null);
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetTitle(char separator = '\t', string prefix = "")
        {
            prefix += "PledgeeCompany.";
            return string.Format("{1}{2}{0}{1}{3}{0}{1}{4}{0}{1}{5}{0}{1}{6}{0}{1}{7}",
                separator,
                prefix,
                "fullName",
                "shortName",
                "address",
                "inn",
                "ogrn",
                "email"
                );
        }
    }
    public class MortgagorPerson
    {
        public string name { get; set; }
        public string lastName { get; set; }
        public string secondName { get; set; }
        public string inn { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}", separator,
                name?.Replace("\t", "").Replace("\r", "").Replace("\n", "").Trim(),
                lastName?.Replace("\t", "").Replace("\r", "").Replace("\n", "").Trim(),
                secondName?.Replace("\t", "").Replace("\r", "").Replace("\n", "").Trim(),
                inn?.Replace("\t", "").Replace("\r", "").Replace("\n", "").Trim()
                );
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
            prefix += "MortgagorPerson.";
            return string.Format("{1}{2}{0}{1}{3}{0}{1}{4}{0}{1}{5}",
                separator,
                prefix,
                "name",
                "lastName",
                "secondName",
                "inn"
                );
        }
    }
    public static class Metods
    {
        public static void Write(this List<SFactMessages> sFactMessages, string name = "SFactMessages.txt", bool append = false, char separator = '\t')
        {
            using (StreamWriter sw = new StreamWriter(name, append, Encoding.UTF8))
            {
                if (!append) { sw.WriteLine(SFactMessages.GetTitle(separator)); }
                foreach (SFactMessages sFact in sFactMessages)
                {
                    sw.WriteLine(sFact.GetStr(separator));
                }
            }
        }
        public static string GetStr(this List<Subject> subjects, char separator = '~', char separatorRow = ';')
        {
            string res = "";
            foreach (Subject subject in subjects)
            {
                res += subject.GetStr(separator) + separatorRow;
            }
            return res;
        }
        public static string GetStr(this List<PledgeeCompany> pledgeeCompanies, char separator = '~', char separatorRow = ';')
        {
            string res = "";
            foreach (PledgeeCompany pledgeeCompany in pledgeeCompanies)
            {
                res += pledgeeCompany.GetStr(separator) + separatorRow;
            }
            return res;
        }
        public static string GetStr(this List<MortgagorPerson> mortgagorPeople, char separator = '~', char separatorRow = ';')
        {
            string res = "";
            foreach (MortgagorPerson mortgagorPerson in mortgagorPeople)
            {
                res += mortgagorPerson.GetStr(separator) + separatorRow;
            }
            return res;
        }

        public static string GetTitle(this List<PledgeeCompany> pledgeeCompanies, char separator = '~', string prefix = "")
        {

            prefix += "PledgeeCompany.";
            return string.Format("{1}{2}{0}{1}{3}{0}{1}{4}{0}{1}{5}{0}{1}{6}{0}{1}{7}",
                separator,
                prefix,
                "fullName",
                "shortName",
                "address",
                "inn",
                "ogrn",
                "email"
                );
        }
        public static string GetTitle(this List<MortgagorPerson> mortgagorPeople, char separator = '~', string prefix = "")
        {
            prefix += "MortgagorPerson.";
            return string.Format("{1}{2}{0}{1}{3}{0}{1}{4}{0}{1}{5}",
                separator,
                prefix,
                "name",
                "lastName",
                "secondName",
                "inn"
                );
        }
    }
}
