using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Security.AccessControl;
using FileGenerationModule.helper;
using System.Reflection;

namespace FileGenerationModule
{
    /// <summary>
    /// This class reads data, creates HTML files
    /// </summary>
    public class FileGenerationController
    {
        #region Variables and properties

        //Destination xml in string format
        string strDestinationXML = string.Empty;
        public string DestinationXML
        {
            get { return strDestinationXML; }
            set { strDestinationXML = value; }
        }
        string strTaxonomyXML = string.Empty;

        public string TaxonomyXML
        {
            get { return strTaxonomyXML; }
            set { strTaxonomyXML = value; }
        }

        CreateParent objCreateParent = new CreateParent();
        CreateChild objCreateChild = new CreateChild();
        private readonly string strCssDirectory = "static";
        List<string> lstParentChain = new List<string>();
        string strHtmlContent = string.Empty;
        private bool bErrorFlag = false;

        #endregion

        /// <summary>
        /// This contructor takes xml data passed on from files
        /// </summary>
        /// <param name="strDesXml">Destination's XML</param>
        /// <param name="strTaxXml">Taxonomy's XML</param>
        /// /// <param name="strOutputDir">Output directory path</param>
        public FileGenerationController(string strDesXml, string strTaxXml,string strOutputDir)
        {
            //Check the write access to specified output directory
            try
            {
                Console.WriteLine("Checking User privilages on output directory.");

                //Checking if the directory path has \ in end if not add
                strOutputDir = strOutputDir.EndsWith("\\") ? strOutputDir : strOutputDir + "\\";

                //check permissions on files and directories
                DirectorySecurity dsPermission = Directory.GetAccessControl(strOutputDir);
                if (dsPermission == null)
                {
                    Console.WriteLine("User does not have write access on the specified output directory or the directory does not exists.");                    
                }
                else
                {
                    Console.WriteLine("Done.");
                    //Create a backup of folder before writing
                    //TODO
                    try
                    {
                        //Check if the files have root elements of expected type
                        if (!(strDesXml.Contains("<destinations>") && strTaxXml.Contains("<taxonomy>")))
                        {
                            throw new Exception("XML files are not having expected data. Please upload correct format files.");
                        }
                        else
                        {
                            strDestinationXML = strDesXml;
                            strTaxonomyXML = strTaxXml;
                            Helper.strOutputDir = strOutputDir;

                            Load();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine( "Exception :" +ex.Message);
                Console.ReadLine();
            }            
        }

        /// <summary>
        /// This method is called from ctor to extract and loop the destination list
        /// </summary>
        private void Load()
        {
            if (strDestinationXML == null || strTaxonomyXML == null)
            {
                Console.WriteLine("The Xml files do not have data.");                
            }
            else
            {
                //Get the node list from xml which is used to fetch all destinations data
                List<DestinationTree> lstDestinations = Helper.LoadDestination(XDocument.Parse(strTaxonomyXML).Descendants("taxonomy").Elements("node"));

                //Create default css folder and copy 'all.css' file from project to location
                if (!Directory.Exists(Helper.strOutputDir + strCssDirectory))
                {
                    Directory.CreateDirectory(Helper.strOutputDir + strCssDirectory);
                    File.Copy(Helper.GetRootDirectory() + "\\css\\all.css", Helper.strOutputDir + strCssDirectory + "\\all.css");
                }

                //Create the home page for the application which would be in this case World.html. This is to include other destinations like Asia, America, Australia...
                objCreateParent.CreateHome(lstDestinations);
                
                //Read the destinations list, call the recursive function to create files in nested folder structure
                ReadandCreateFiles(lstDestinations, resources.Resources.ParentTaxonomy);

                if (bErrorFlag)
                {
                    Console.WriteLine("Error in file generation.");                    
                }
                else
                {
                    Console.WriteLine("File generation successfull. Press any key to launch application.");
                    Console.ReadLine();
                    //At the end of processing of data and file creation. Show successful message and launch the home page for the application
                    System.Diagnostics.Process.Start(Helper.strOutputDir + resources.Resources.ParentTaxonomy + ".html");
                }
            }            
        }               

        /// <summary>
        /// Recursive function to create parent and child elements
        /// </summary>
        /// <param name="lstDesTree">list of destination under a parent node</param>
        /// <param name="strParentDes">Parent destination name</param>
        private void ReadandCreateFiles(List<DestinationTree> lstDesTree, string strParentDes)
        {
            //Iterate through all destinations
            foreach (DestinationTree item in lstDesTree)
            {
                string strName = item.DestinationName;
                string strGeoId = item.GeoId;
                //If there are child destinations; create a new directory for this location, navigate into the directory and create child pages in it.
                if (item.SubDestination.Count > 0)
                {
                    lstParentChain.Add(item.DestinationName);
                    if(!Directory.Exists(Helper.strOutputDir + string.Join("\\",lstParentChain)))
                       Directory.CreateDirectory(Helper.strOutputDir + string.Join("\\",lstParentChain));

                    using (FileStream fs = new FileStream(Helper.strOutputDir + string.Join("\\",lstParentChain.Except(new List<string> { item.DestinationName })) +"\\"+ strName + ".html", FileMode.Create))
                    {
                        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                        {
                            strHtmlContent = objCreateParent.Create(strDestinationXML, strName, item.GeoId, strParentDes, lstParentChain, item.SubDestination);
                            if (strHtmlContent != string.Empty)
                            {
                                w.WriteLine(strHtmlContent);
                                //Console.WriteLine("File created:" + strName + ".html");
                            }
                            else
                            {
                                //Set the flag if the template not as per expected format and exit the loop
                                bErrorFlag = true;
                                break;
                            }
                        }
                    }
                    //make a reccursive call for child destinations
                    ReadandCreateFiles(item.SubDestination, item.DestinationName);
                    
                    //After completion of child loop remove the current destination from parent chain
                    lstParentChain.RemoveAt(lstParentChain.Count - 1);
                }
                else
                {
                    //If there is no child element create a new page under the parent 
                    using (FileStream fs = new FileStream(Helper.strOutputDir + string.Join("\\",lstParentChain)+"\\"+ strName + ".html", FileMode.Create))
                    {
                        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                        {
                            strHtmlContent = objCreateChild.Create(strDestinationXML, strName, item.GeoId, strParentDes, lstParentChain);
                            //w.WriteLine(objCreateChild.Create(strDestinationXML, strName, item.GeoId, strParentDes, lstParentChain));
                            if (strHtmlContent != string.Empty)
                            { 
                                w.WriteLine(strHtmlContent); 
                                //Console.WriteLine("File created:" + strName + ".html"); 
                            }
                            else
                            {
                                //Set the flag if the template not as per expected format and exit the loop
                                bErrorFlag = true;
                                break;
                            }
                        }
                    }
                }
            }
        }                
    }   
}
