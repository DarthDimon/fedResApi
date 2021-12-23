using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace fedResApi2
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondName { get; set; }
        public DateTime? BirtDay { get; set; } = null;
        public string GetFullNameStr()
        {
            return string.Format("{0} {1} {2}", LastName, FirstName, SecondName);
        }
    }
    static class PersonM
    {
        public static List<Person> GetFromTxt(this List<Person> people,  string path ,bool getBirtDay=false)
        {
            using(StreamReader sr=new StreamReader(path))
            {

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] col = line.Trim().Split('\t');
                    people.Add(new Person()
                    {
                        LastName = col[0],
                        FirstName = col[1],
                        SecondName = col[2],
                        BirtDay = getBirtDay ? Convert.ToDateTime(col[3]) : Convert.ToDateTime(null)
                    });
                }
            }
            return people;
        }
    }
}
