using FileGenerationModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LPHelper = FileGenerationModule.helper;
using System.IO;

namespace LPUnitTest
{   
    
    /// <summary>
    ///This is a test class for CreateChildTest and is intended
    ///to contain all CreateChildTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CreateChildTest
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
        ///A test for CreateChild Constructor
        ///</summary>
        //[TestMethod()]
        //public void CreateChildConstructorTest()
        //{
        //    CreateChild target = new CreateChild();
        //    Assert.Inconclusive("TODO: Implement code to verify target");
        //}

        /// <summary>
        ///A test for Create
        ///</summary>
        [TestMethod()]
        public void CreateTest()
        {
            CreateChild target = new CreateChild(); 
            string strDestinationXml = XDocument.Load(Resource1.DestinationXml).ToString();
            string name = "Royal Natal National Park";
            string strGeoId = "355625";
            string strParentDestination = "The Drakensberg"; 
            List<DestinationTree> lstChild = new List<DestinationTree> { new DestinationTree { } };
            //string expected = string.Empty; 
            string actual;
            List<string> lstParentChain = new List<string>();
            lstParentChain.Add("Africa");
            lstParentChain.Add("South Africa");
            lstParentChain.Add("The Drakensberg");
                        
            actual = target.Create(strDestinationXml, name, strGeoId, strParentDestination,lstParentChain, lstChild);
            Assert.IsTrue(actual.Contains("Royal Natal National Park"));
            Assert.IsTrue(actual.Contains("The Drakensberg"));
            Assert.IsTrue(actual.Contains("South Africa"));
            
        }
    }
}
