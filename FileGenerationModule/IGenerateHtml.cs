using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileGenerationModule
{
    /// <summary>
    /// This interface defines the structure for create functionality for parent and child destinations
    /// </summary>
    interface IGenerateHtml
    {
        string Create(string strDestinationXml, string name, string strGeoId, string strParentDestination,List<string> lstParentChain , List<DestinationTree> lstChild = null);
    }
}
