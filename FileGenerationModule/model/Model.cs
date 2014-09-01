using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileGenerationModule
{
    /// <summary>
    /// This class binds the destination's data as per the schema generated.
    /// </summary>
    public class Model
    {
        private destinationsDestinationHistory histroy;

        public destinationsDestinationHistory Histroy
        {
            get { return histroy; }
            set { histroy = value; }
        }
        private destinationsDestinationIntroductory intro;

        public destinationsDestinationIntroductory Intro
        {
            get { return intro; }
            set { intro = value; }
        }
        private destinationsDestinationPractical_information practicalInfo;

        public destinationsDestinationPractical_information PracticalInfo
        {
            get { return practicalInfo; }
            set { practicalInfo = value; }
        }
        private destinationsDestinationTransport transport;

        public destinationsDestinationTransport Transport
        {
            get { return transport; }
            set { transport = value; }
        }
        private destinationsDestinationWeather weather;

        public destinationsDestinationWeather Weather
        {
            get { return weather; }
            set { weather = value; }
        }
        private destinationsDestinationWork_live_study work_live_study;

        public destinationsDestinationWork_live_study Work_live_study
        {
            get { return work_live_study; }
            set { work_live_study = value; }
        }
    }
}
