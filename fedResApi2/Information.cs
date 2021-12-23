using System;
using System.Collections.Generic;
using System.Text;

namespace fedResApi2
{
    public class Information
    {
        public Info info { get; set; }
        public Roles roles { get; set; }
        public string guid { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}", separator, info.GetStr(), roles.GetStr(), guid);
        }
        /// <summary>
        /// возвращает пустую строку
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetNullStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}", separator, Info.GetNullStr(), Roles.GetNullStr(), null);
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetTitle(char separator = '\t', string prefix = "")
        {
            return string.Format("{1}{2}{0}{1}{3}{0}{1}{4}",
                separator,
                prefix,
                Info.GetTitle(),
                Roles.GetTitle(),
                "guid");
        }
    }
    public class Info
    {
        public string fullName { get; set; }
        public string address { get; set; }
        public List<string> email { get; set; }
        public List<string> phone { get; set; }
        public string inn { get; set; }
        public string snils { get; set; }
        public DateTime birthdateBankruptcy { get; set; }
        public string birthplaceBankruptcy { get; set; }
        public bool isBankruptcyMoratoriumParticipant { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}", separator,
                fullName,
                address,
                email.Count != 0 ? email.ToStr(';') : null,
                phone.Count != 0 ? phone.ToStr(';') : null,
                inn,
                snils,
                birthdateBankruptcy,
                birthplaceBankruptcy,
                isBankruptcyMoratoriumParticipant
                );
        }
        /// <summary>
        /// возвращает пустую строку
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetNullStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}", separator, null, null, null, null, null, null, null, null, null);
        }
        /// <summary>
        /// возвращает строку заголовков класса через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public static string GetTitle(char separator = '\t', string prefix = "")
        {
            prefix += "SroInfo.";
            return string.Format("{1}{2}{0}{1}{3}{0}{1}{4}{0}{1}{5}{0}{1}{6}{0}{1}{7}{0}{1}{8}{0}{1}{9}{0}{1}{10}",
                separator,
                prefix,
                "fullName",
                "address",
                "email",
                "phone",
                "inn",
                "snils",
                "birthdateBankruptcy",
                "birthplaceBankruptcy",
                "isBankruptcyMoratoriumParticipant"
                );
        }
    }
    public class Roles
    {
        public bool isAppraiser { get; set; }
        public bool isArbitrManager { get; set; }
        public bool isTradeOrg { get; set; }
        public bool isIndividualEntrepreneur { get; set; }
        public bool isPersonBankrupt { get; set; }
        /// <summary>
        /// возвращает класс в строке через разделитель
        /// </summary>
        /// <param name="separator">разделитель</param>
        /// <returns></returns>
        public string GetStr(char separator = '\t')
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}", separator, isAppraiser, isArbitrManager, isTradeOrg, isIndividualEntrepreneur, isPersonBankrupt);
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
            prefix += "Roles.";
            return string.Format("{1}{2}{0}{1}{3}{0}{1}{4}{0}{1}{5}{0}{1}{6}",
                separator,
                prefix,
                "isAppraiser",
                "isArbitrManager",
                "isTradeOrg",
                "isIndividualEntrepreneur",
                "isPersonBankrupt");
        }

    }
    public static class InformationM
    {
        public static string ToStr(this List<string> list, char separator)
        {
            string strRes = "";
            foreach (string str in list)
            {
                strRes += str + separator;

            }
            return strRes;
        }
    }

}
