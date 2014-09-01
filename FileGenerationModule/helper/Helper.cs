using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Reflection;

namespace FileGenerationModule.helper
{
    /// <summary>
    /// This is a static class with methods and variables that provide help to controller class
    /// </summary>
    public static class Helper
    {
        #region Variables

        public static string strOutputDir;

        //public static Dictionary<string, string> LPConstants = new Dictionary<string, string>()
        //{
        //   { "DestinationNode","<destinations>"}
        //};

        #endregion

        /// <summary>
        /// Recursive function to load destinations from XML
        /// </summary>
        /// <param name="des">XML node - XElement</param>
        /// <returns>List of  type 'DestinationTree'</returns>
        public static List<DestinationTree> LoadDestination(IEnumerable<XElement> des)
        {
            return des.Select(x => new DestinationTree()
            {
                DestinationName = x.Element("node_name").Value,
                GeoId = x.Attribute("geo_id").Value,
                SubDestination = LoadDestination(x.Elements("node"))
            }).ToList();
        }

        /// <summary>
        /// This method creates a HTML content by replacing the placeholders with data
        /// </summary>
        /// <param name="name">current destination name</param>
        /// <param name="lstChild">list of child destinations that will apprear in right nav</param>
        /// <returns>string - HTML content for a file</returns>
        public static string GenerateHtml(string name, List<DestinationTree> lstChild)
        {
            StringBuilder strTemplate = new StringBuilder(Helper.GetTemplateFile());
            strTemplate = strTemplate.Replace("{DESTINATION NAME}", name);

            strTemplate = strTemplate.Replace("{CONTENT}", "<H2>Welcome to Lonely Planet.</H2>");

            if (lstChild != null)
            {
                StringBuilder sbSidebarNav = new StringBuilder();
                sbSidebarNav.Append("<ul>");
                foreach (DestinationTree childNode in lstChild)
                {
                    sbSidebarNav.Append("<li><a href=\"" + strOutputDir + childNode.DestinationName + ".html" + "\">" + childNode.DestinationName + "</a></li>");
                }
                sbSidebarNav.Append("</ul>");
                strTemplate = strTemplate.Replace("{NAVIGATION}", sbSidebarNav.ToString());
            }
            else
                strTemplate = strTemplate.Replace("{NAVIGATION}", "No linked destination.");

            //Parent nav
            strTemplate = strTemplate.Replace("{PARENT DESTINATION NAME}", "");
            strTemplate = strTemplate.Replace("{CSS PATH}", Helper.strOutputDir);
            return strTemplate.ToString();
        }

        /// <summary>
        /// This method creates a HTML content by replacing the placeholders with data
        /// </summary>
        /// <param name="strDestinationXml">complete xml of destinations</param>
        /// <param name="name">name of the current destination</param>
        /// <param name="strGeoId">current destinations geoid</param>
        /// <param name="strParentDestination">parent destination name</param>
        /// <param name="lstChild">list of destination</param>
        /// <returns>string - HTML content for a file</returns>
        public static string GenerateHtml(string strDestinationXml, string name, string strGeoId, string strParentDestination, List<DestinationTree> lstChild = null)
        {
            try
            {

                StringBuilder strTemplate = new StringBuilder(Helper.GetTemplateFile());
                strTemplate = strTemplate.Replace("{DESTINATION NAME}", name);

                Model MainBodyContentModel = XDocument.Parse(strDestinationXml).Descendants("destinations").Elements("destination")
                    .Where(x => x.Attribute("title").Value == name && x.Attribute("atlas_id").Value == strGeoId)
                    .Select(x => new Model
                    {
                        Intro = new destinationsDestinationIntroductory
                        {
                            introduction = new destinationsDestinationIntroductoryIntroduction
                            {
                                overview = x.Elements("introductory").Elements("introduction")
                                  .Select(intro => intro.Element("overview").Value).First()
                            }
                        }
                    ,
                        PracticalInfo = new destinationsDestinationPractical_information
                        {
                            health_and_safety = new destinationsDestinationPractical_informationHealth_and_safety
                            {
                                before_you_go = x.Elements("practical_information").Elements("health_and_safety")
                                      .Select(intro => intro.Element("before_you_go") != null ? intro.Element("before_you_go").Value : "").ToArray()
                            }
                        }
                    }).First();

                strTemplate = strTemplate.Replace("{CONTENT}", "<H2>INTRODUCTION</H2><br/><H4>OVERVIEW</H4><br/>" + MainBodyContentModel.Intro.introduction.overview +
                                                    "<br/><H2>PRACTICAL INFORMATION</H2><br/><H4>HEALTH SAFETY: Before you go</H4><br/>" + string.Join("<br/>", MainBodyContentModel.PracticalInfo.health_and_safety.before_you_go));

                if (lstChild != null)
                {
                    StringBuilder sbSidebarNav = new StringBuilder();
                    sbSidebarNav.Append("<ul>");
                    foreach (DestinationTree childNode in lstChild)
                    {
                        sbSidebarNav.Append("<li><a href=\"" + strOutputDir + childNode.DestinationName + ".html" + "\">" + childNode.DestinationName + "</a></li>");
                    }
                    sbSidebarNav.Append("</ul>");
                    strTemplate = strTemplate.Replace("{NAVIGATION}", sbSidebarNav.ToString());
                }
                else
                    strTemplate = strTemplate.Replace("{NAVIGATION}", "No linked destination.");

                //Parent nav
                strTemplate = strTemplate.Replace("{PARENT DESTINATION NAME}", "<li class=\"first\"><a href=\"" + strOutputDir + strParentDestination + ".html" + "\">" + strParentDestination + "</a></li>");

                return strTemplate.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex);
                return string.Empty;
            }
        }

        public static string GetTemplateFile()
        {
            string strCurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Substring(6);

            return File.ReadAllText(Directory.GetParent(Directory.GetParent(strCurrentDirectory).FullName).FullName + "\\template\\example.html");
            
        }

        public static string GetRootDirectory()
        {
            string strCurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Substring(6);

            return Directory.GetParent(Directory.GetParent(strCurrentDirectory).FullName).FullName;

        }
    }
}
