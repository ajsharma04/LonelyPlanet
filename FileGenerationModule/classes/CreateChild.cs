using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using FileGenerationModule.helper;

namespace FileGenerationModule
{
    /// <summary>
    /// This class generates html content for the child pages
    /// </summary>
    public class CreateChild : IGenerateHtml
    {
        public string Create(string strDestinationXml, string name, string strGeoId, string strParentDestination, List<string> lstParentChain, List<DestinationTree> lstChild = null)
        {
            try
            {
                StringBuilder strTemplate = new StringBuilder(Helper.GetTemplateFile());

                if (strTemplate.ToString().Contains("{DESTINATION NAME}") &&
                    strTemplate.ToString().Contains("{CONTENT}") &&
                    strTemplate.ToString().Contains("{NAVIGATION}") &&
                    strTemplate.ToString().Contains("{CSS PATH}") &&
                    strTemplate.ToString().Contains("{PARENT DESTINATION NAME}"))
                {
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

                    StringBuilder sbPageContent = new StringBuilder();
                    if (MainBodyContentModel.Intro.introduction.overview != "")
                        sbPageContent.Append("<H2>INTRODUCTION</H2><br/><H4>OVERVIEW</H4><br/>" + MainBodyContentModel.Intro.introduction.overview);
                    if (MainBodyContentModel.PracticalInfo.health_and_safety.before_you_go.Length > 0)
                        sbPageContent.Append("<br/><H2>PRACTICAL INFORMATION</H2><br/><H4>HEALTH SAFETY: Before you go</H4><br/>" + string.Join("<br/>", MainBodyContentModel.PracticalInfo.health_and_safety.before_you_go));

                    strTemplate = strTemplate.Replace("{CONTENT}", sbPageContent.ToString());

                    //strTemplate = strTemplate.Replace("{CONTENT}", "<H2>INTRODUCTION</H2><br/><H4>OVERVIEW</H4><br/>" + MainBodyContentModel.Intro.introduction.overview +
                    //                                    "<br/><H2>PRACTICAL INFORMATION</H2><br/><H4>HEALTH SAFETY: Before you go</H4><br/>" + string.Join("<br/>", MainBodyContentModel.PracticalInfo.health_and_safety.before_you_go));

                    strTemplate = strTemplate.Replace("{NAVIGATION}", "No linked destination.");

                    //CSS
                    strTemplate = strTemplate.Replace("{CSS PATH}", Helper.strOutputDir);

                    //Parent nav
                    strTemplate = strTemplate.Replace("{PARENT DESTINATION NAME}", "<li class=\"first\"><a href=\"" + Helper.strOutputDir + string.Join("\\", lstParentChain.Except(new List<string> { strParentDestination })) + "\\" + strParentDestination + ".html" + "\">" + strParentDestination + "</a></li>");

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
    }
}
