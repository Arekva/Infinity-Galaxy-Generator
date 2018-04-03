using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infinity.Datas.Enums.Body;

namespace Infinity.Datas.Body
{
    class ScaledVersion
    {
        public ScaledVersionTypes Type { get; set; }

        //Material
        public string Texture { get; set; }
        public string Normals { get; set; }
    }
}
