using LPMain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using LPHelper = FileGenerationModule.helper;

namespace LPUnitTest
{
    
    
    /// <summary>
    ///This is a test class for ProgramTest and is intended
    ///to contain all ProgramTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProgramTest
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
        ///A test for Main
        ///</summary>
        [TestMethod()]
        [DeploymentItem("LPMain.exe")]
        public void MainTest()
        {
            string[] args = {Resource1.DestinationXml,Resource1.TaxonomyXml,Resource1.OutputPath};
            Program_Accessor.Main(args);
            Assert.IsTrue(Directory.GetFiles(Resource1.OutputPath).Length >0);
        }
    }
}
