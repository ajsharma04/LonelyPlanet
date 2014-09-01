using FileGenerationModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using LPHelper = FileGenerationModule.helper;
using System.IO;

namespace LPUnitTest
{
    
    
    /// <summary>
    ///This is a test class for DestinationTreeTest and is intended
    ///to contain all DestinationTreeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DestinationTreeTest
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
        ///A test for DescriptionModel
        ///</summary>
        [TestMethod()]
        public void DescriptionModelTest()
        {
            DestinationTree target = new DestinationTree();
            Model expected = new Model { Intro = new destinationsDestinationIntroductory { introduction = new destinationsDestinationIntroductoryIntroduction { overview = "This is test."} } };
            Model actual;
            target.DescriptionModel = expected;
            actual = target.DescriptionModel;
            Assert.AreEqual(expected, actual);            
        }

        /// <summary>
        ///A test for DestinationName
        ///</summary>
        [TestMethod()]
        public void DestinationNameTest()
        {
            DestinationTree target = new DestinationTree(); 
            string expected = "Sudan"; 
            string actual;
            target.DestinationName = expected;
            actual = target.DestinationName;
            Assert.AreEqual(expected, actual);            
        }

        /// <summary>
        ///A test for GeoId
        ///</summary>
        [TestMethod()]
        public void GeoIdTest()
        {
            DestinationTree target = new DestinationTree(); 
            string expected = "50345";
            string actual;
            target.GeoId = expected;
            actual = target.GeoId;
            Assert.AreEqual(expected, actual);            
        }

        /// <summary>
        ///A test for SubDestination
        ///</summary>
        [TestMethod()]
        public void SubDestinationTest()
        {
            DestinationTree target = new DestinationTree();
            List<DestinationTree> expected = new List<DestinationTree> { new DestinationTree {DestinationName = "Loc1",GeoId = "12345",DescriptionModel = new Model{}, SubDestination = new List<DestinationTree>{new DestinationTree {DestinationName = "loc2",GeoId="12346"}}}};
            List<DestinationTree> actual;
            target.SubDestination = expected;
            actual = target.SubDestination;
            Assert.AreEqual(expected, actual);            
        }
    }
}
