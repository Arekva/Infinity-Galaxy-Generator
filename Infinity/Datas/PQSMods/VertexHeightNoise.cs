using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infinity.Datas.PQSMods.Enums;
namespace Infinity.Datas.PQSMods
{
    class VertexHeightNoise
    {
        public double deformity { get; set; }
        public double frequency { get; set; }
        public double octaves { get; set; }
        public double persistence { get; set; }
        public double seed { get; set; }
        public noiseType noiseType { get; set; }
        public mode mode { get; set; }
        public double lacunarity { get; set; }
        public int order { get; set; }
        public bool enabled { get; set; }
        public string name { get; set; }
        public int index { get; set; }
    }
}
