using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileGenerationModule
{
    /// <summary>
    /// This model class holds the data for a node which includes its name, geoid, data and child destinations
    /// </summary>
    public class DestinationTree
    {
        public string DestinationName { get; set; }
        public string GeoId { get; set; }
        public Model DescriptionModel { get; set; }
        public List<DestinationTree> SubDestination { get; set; }
    }
}
