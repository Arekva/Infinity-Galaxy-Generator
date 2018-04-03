using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infinity.Datas.Enums;

namespace Infinity.Datas.Body
{
    class Orbit
    {
        public string ReferenceBody { get; set; }
        public double Inclination { get; set; }
        public double Eccentricity { get; set; }
        public double SemiMajorAxis { get; set; }
        public double LongitudeOfAscendingNode { get; set; }
        public double ArgumentOfPeriapsis { get; set; }
        public double Epoch { get; set; }
        public double meanAnomalyAtEpoch { get; set; }
        public string Color { get; set; }
    }
}
