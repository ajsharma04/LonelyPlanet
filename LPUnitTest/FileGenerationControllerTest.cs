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
    ///This is a test class for FileGenerationControllerTest and is intended
    ///to contain all FileGenerationControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FileGenerationControllerTest
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
        ///A test for FileGenerationController Constructor
        ///</summary>
        [TestMethod()]
        public void FileGenerationControllerConstructorTest_P()
        {
            string strDesXml = XDocument.Load(Resource1.DestinationXml).ToString();
            string strTaxXml = XDocument.Load(Resource1.TaxonomyXml).ToString();
            string strOutputDir = Resource1.OutputPath;
            FileGenerationController target = new FileGenerationController(strDesXml, strTaxXml, strOutputDir);
            Assert.AreEqual(target.DestinationXML, strDesXml);
            Assert.AreEqual(target.TaxonomyXML, strTaxXml);
        }

        /// <summary>
        ///A negative test for FileGenerationController Constructor
        ///</summary>
        [TestMethod()]
        public void FileGenerationControllerConstructorTestWithWrongXmlPath_N()
        {
            string strDesXml = @"SA:\";
            string strTaxXml = XDocument.Load(Resource1.TaxonomyXml).ToString();
            string strOutputDir = Resource1.OutputPath;
            FileGenerationController target = new FileGenerationController(strDesXml, strTaxXml, strOutputDir);
            Assert.AreNotEqual(target.DestinationXML, strDesXml);
            Assert.AreNotEqual(target.TaxonomyXML, strTaxXml);
        }

        /// <summary>
        ///A negative test for FileGenerationController Constructor
        ///</summary>
        [TestMethod()]
        public void FileGenerationControllerConstructorTestWithWrongOutputPath_N()
        {
            string strDesXml = XDocument.Load(Resource1.DestinationXml).ToString();
            string strTaxXml = XDocument.Load(Resource1.TaxonomyXml).ToString();
            string strOutputDir = "ABC";
            FileGenerationController target = new FileGenerationController(strDesXml, strTaxXml, strOutputDir);
            Assert.AreNotEqual(target.DestinationXML, strDesXml);
            Assert.AreNotEqual(target.TaxonomyXML, strTaxXml);
        }

        /// <summary>
        ///A test for Load
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FileGenerationModule.dll")]
        public void LoadTest()
        {
            PrivateObject param0 = new PrivateObject(new FileGenerationController(XDocument.Load(Resource1.DestinationXml).ToString(), XDocument.Load(Resource1.TaxonomyXml).ToString(), Resource1.OutputPath)); // TODO: Initialize to an appropriate value
            FileGenerationController_Accessor target = new FileGenerationController_Accessor(param0);
            target.Load();
            Assert.IsFalse(target.bErrorFlag);
        }
    }
}
