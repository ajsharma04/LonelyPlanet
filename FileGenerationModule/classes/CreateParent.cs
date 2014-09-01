using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.IO;
using FileGenerationModule.helper;

namespace FileGenerationModule
{
    /// <summary>
    /// This class creates parent files
    /// </summary>
    public class CreateParent : IGenerateHtml
    {
        List<string> lstParentNodeNames = new List<string>();

        public string Create(string strDestinationXml, string name, string strGeoId, string strParentDestination, List<string> lstParentChain, List<DestinationTree> lstChild = null)
        {
            try
            {

                //Get the html template from the project. This html has all the placeholders which will replace 
                StringBuilder strTemplate = new StringBuilder(Helper.GetTemplateFile());

                if (strTemplate.ToString().Contains("{DESTINATION NAME}") &&
                    strTemplate.ToString().Contains("{CONTENT}") &&
                    strTemplate.ToString().Contains("{NAVIGATION}") &&
                    strTemplate.ToString().Contains("{CSS PATH}") &&
                    strTemplate.ToString().Contains("{PARENT DESTINATION NAME}"))
                {
                    //replace the destination name
                    strTemplate = strTemplate.Replace("{DESTINATION NAME}", name);

                    //get the data for the current node based on the geoid and name -- currently getting on overview and before you go data
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

                    //replace the content place holder
                    StringBuilder sbPageContent = new StringBuilder();
                    if (MainBodyContentModel.Intro.introduction.overview != "")
                        sbPageContent.Append("<H2>INTRODUCTION</H2><br/><H4>OVERVIEW</H4><br/>" + MainBodyContentModel.Intro.introduction.overview);
                    if (MainBodyContentModel.PracticalInfo.health_and_safety.before_you_go.Length > 0)
                        sbPageContent.Append("<br/><H2>PRACTICAL INFORMATION</H2><br/><H4>HEALTH SAFETY: Before you go</H4><br/>" + string.Join("<br/>", MainBodyContentModel.PracticalInfo.health_and_safety.before_you_go));
                    strTemplate = strTemplate.Replace("{CONTENT}", sbPageContent.ToString());

                    //strTemplate = strTemplate.Replace("{CONTENT}", "<H2>INTRODUCTION</H2><br/><H4>OVERVIEW</H4><br/>" + MainBodyContentModel.Intro.introduction.overview +
                    //                                    "<br/><H2>PRACTICAL INFORMATION</H2><br/><H4>HEALTH SAFETY: Before you go</H4><br/>" + string.Join("<br/>", MainBodyContentModel.PracticalInfo.health_and_safety.before_you_go));

                    //Create the html data for side nav
                    StringBuilder sbSidebarNav = new StringBuilder();
                    sbSidebarNav.Append("<ul>");
                    foreach (DestinationTree childNode in lstChild)
                    {
                        sbSidebarNav.Append("<li><a href=\"" + Helper.strOutputDir + string.Join("\\", lstParentChain) + "\\" + childNode.DestinationName + ".html" + "\">" + childNode.DestinationName + "</a></li>");
                    }
                    sbSidebarNav.Append("</ul>");
                    strTemplate = strTemplate.Replace("{NAVIGATION}", sbSidebarNav.ToString());

                    //replace relative css path with absolute path
                    strTemplate = strTemplate.Replace("{CSS PATH}", Helper.strOutputDir);

                    //Parent nav
                    strTemplate = strTemplate.Replace("{PARENT DESTINATION NAME}", "<li class=\"first\"><a href=\"" + Helper.strOutputDir + string.Join("\\", lstParentChain.Except(new List<string> { name, strParentDestination })) + "\\" + strParentDestination + ".html" + "\">" + strParentDestination + "</a></li>");

                    return strTemplate.ToString();
                }
                else
                {
                    Console.WriteLine("Not a valid template file.");
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return string.Empty;
            }

        }

        public void CreateHome(List<DestinationTree> lstDestinations)
        {
            using (FileStream fs = new FileStream(Helper.strOutputDir + resources.Resources.ParentTaxonomy + ".html", FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(Helper.GenerateHtml(resources.Resources.ParentTaxonomy, lstDestinations));
                }
            }
        }
    }
}
