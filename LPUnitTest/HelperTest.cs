using FileGenerationModule.helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FileGenerationModule;
using System.Collections.Generic;
using System.Xml.Linq;
using LPHelper = FileGenerationModule.helper;
using System.IO;

namespace LPUnitTest
{
    
    
    /// <summary>
    ///This is a test class for HelperTest and is intended
    ///to contain all HelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class HelperTest
    {
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            LPHelper.Helper.strOutputDir = Resource1.OutputPath;
            System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo(Resource1.OutputPath);

            foreach (FileInfo file in downloadedMessageInfo.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in downloadedMessageInfo.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {

        }
        //
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo(Resource1.OutputPath);

            foreach (FileInfo file in downloadedMessageInfo.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in downloadedMessageInfo.GetDirectories())
            {
                dir.Delete(true);
            }

        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }        

        /// <summary>
        ///A test for GenerateHtml
        ///</summary>
        [TestMethod()]
        public void GenerateHtmlTest_N()
        {
            string strDestinationXml = XDocument.Load(Resource1.DestinationXml).ToString();  // TODO: Initialize to an appropriate value
            string name = "Charlie"; // TODO: Initialize to an appropriate value
            string strGeoId = "345"; // TODO: Initialize to an appropriate value
            string strParentDestination = "Romeo"; // TODO: Initialize to an appropriate value
            List<DestinationTree> lstChild = null; // TODO: Initialize to an appropriate value
            //string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = Helper.GenerateHtml(strDestinationXml, name, strGeoId, strParentDestination, lstChild);
            Assert.IsFalse(actual.Contains("Charlie"));
            Assert.IsFalse(actual.Contains("Romeo"));           
        }
        
        /// <summary>
        ///A test for GenerateHtml
        ///</summary>
        [TestMethod()]
        public void GenerateHtmlTest()
        {
            string strDestinationXml = XDocument.Load(Resource1.DestinationXml).ToString();  // TODO: Initialize to an appropriate value
            string name = "Africa"; // TODO: Initialize to an appropriate value
            string strGeoId = "355064"; // TODO: Initialize to an appropriate value
            string strParentDestination = "World"; // TODO: Initialize to an appropriate value
            List<DestinationTree> lstChild = null; // TODO: Initialize to an appropriate value
            //string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = Helper.GenerateHtml(strDestinationXml, name, strGeoId, strParentDestination, lstChild);
            Assert.IsTrue(actual.Contains("Africa"));
            Assert.IsTrue(actual.Contains("World"));
        }

        /// <summary>
        ///A test for GenerateHtml
        ///</summary>
        [TestMethod()]
        public void GenerateHtmlTest1()
        {
            string name = "Alpha"; // TODO: Initialize to an appropriate value
            List<DestinationTree> lstChild = null; // TODO: Initialize to an appropriate value
            //string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = Helper.GenerateHtml(name, lstChild);
            Assert.IsTrue(actual.Contains("Alpha"));
            Assert.IsTrue(actual.Contains("Welcome to Lonely Planet"));            
        }
                
    }
}
