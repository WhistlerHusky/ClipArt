using System.Collections.Generic;
using System.Xml.Linq;

namespace XmlControler
{
    /// <summary>
    /// 
    /// </summary>
    public static class XmlMessage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IList<string> GetNotifyIconMenuList()
        {
            var notifyIconMenuList = new List<string>();

            var notifyXmlDoc = XDocument.Load(
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) +
                @"\..\Data\Xml\NotifyIconMenuList.xml");

            notifyIconMenuList.Add(notifyXmlDoc.Root.Element("Root").Attribute("menuItem1").Value);
            notifyIconMenuList.Add(notifyXmlDoc.Root.Element("Root").Attribute("menuItem2").Value);
            notifyIconMenuList.Add(notifyXmlDoc.Root.Element("Root").Attribute("menuItem3").Value);
           
            return notifyIconMenuList;
        }

        public static void SetNotifyIconMenuList(string exePath)
        {
            var notifyIcon =
                new XDocument(
                    new XComment("NotifyIconMenu"),
                    new XElement(
                        "NotifyIconMenu",
                        new XElement(
                            "Root",
                            new XAttribute("menuItem1", "열기"),
                            new XAttribute("menuItem2", "정보"),
                            new XAttribute("menuItem3", "종료"))));
            notifyIcon.Save(exePath + @"\..\Data\Xml\NotifyIconMenuList.xml");
        }
    }
}
