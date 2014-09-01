using FileGenerationModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using LPHelper=FileGenerationModule.helper;

namespace LPUnitTest
{
    
    
    /// <summary>
    ///This is a test class for CreateParentTest and is intended
    ///to contain all CreateParentTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CreateParentTest
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
        ///A test for Create: This test method tests that the parameters supplied to this methods are correctly replaced with the place holders
        ///</summary>
        [TestMethod()]
        public void CreateTest()
        {
            CreateParent target = new CreateParent(); 
            string strDestinationXml = XDocument.Load(Resource1.DestinationXml).ToString();
            string name = "The Drakensberg";
            string strGeoId = "355624";
            string strParentDestination = "South Africa";
            List<DestinationTree> lstChild = new List<DestinationTree> { new DestinationTree { DestinationName = "Royal Natal National Park", GeoId = "355625", DescriptionModel = new Model { }, SubDestination = null } };
            //string expected = Resource1.TestParentCreateMethodExpected;
            string actual;
            List<string> lstParentChain = new List<string>();
            lstParentChain.Add("Africa");
            lstParentChain.Add("South Africa");

            actual = target.Create(strDestinationXml, name, strGeoId, strParentDestination, lstParentChain, lstChild);
            Assert.IsTrue(actual.Contains("The Drakensberg"));
            Assert.IsTrue(actual.Contains("South Africa"));
            Assert.IsTrue(actual.Contains("Royal Natal National Park"));            
        }

        /// <summary>
        ///A test for CreateHome. This creates a world.html file
        ///</summary>
        [TestMethod()]
        public void CreateHomeTest()
        {
            CreateParent target = new CreateParent();
            List<DestinationTree> lstDestinations = new List<DestinationTree> { new DestinationTree {}}; 
            target.CreateHome(lstDestinations);
            Assert.IsTrue(File.Exists(Resource1.OutputPath+"World.html"));
        }
    }
}
