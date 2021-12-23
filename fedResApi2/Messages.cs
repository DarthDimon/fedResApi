using System;
using System.Collections.Generic;
using System.Text;
using Sfactmessages;
using fedResApi2;

namespace MSG
{
    public class Messages
    {
        public static List<SFactMessages> SFactMessages { get; set; } = new List<SFactMessages>();
        public void AddMessages(PageData pageData)
        {
            switch (pageData.type)
            {
                case "SfactMessage":
                    SFactMessages.Add(new SFactMessages(pageData.guid));
                    break;
            }
        }
        public void AddMessages(List<PageData> pageDatas, int writeLengh = 0)
        {
            if (writeLengh > 0) 
            {
                SFactMessages.Write(append: false); 
            }
            for (int i = 0; i < pageDatas.Count; i++)
            {
                AddMessages(pageDatas[i]);
                if ((writeLengh > 0 && i % writeLengh == 0 && i > 0) || writeLengh == 1)
                {
                    SFactMessages.Write(append: true);
                    SFactMessages.Clear();
                }
            }
        }
    }

}
