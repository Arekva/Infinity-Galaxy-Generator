using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigNodeParser;
using Enums = Infinity.Datas.Enums;
using Body = Infinity.Datas.Body;

namespace Infinity.Generators
{
    public class Planet : Body.Body
    {
        Body.Body body = new Body.Body();
        Body.Template template = new Body.Template();
        Body.Orbit orbit = new Body.Orbit();
        Body.ScaledVersion scaledVersion = new Body.ScaledVersion();
        Datas.PQSMods.VertexHeightNoise vhn = new Datas.PQSMods.VertexHeightNoise();



        ConfigNode MMNode = new ConfigNode("@Kopernicus:FOR[INFINITY]");
        ConfigNode BodyNode = new ConfigNode("Body");
        ConfigNode TemplateNode = new ConfigNode("Template");
        ConfigNode OrbitNode = new ConfigNode("Orbit");
        ConfigNode ScaledVersionNode = new ConfigNode("ScaledVersion");
        ConfigNode ScaledVersionMaterialNode = new ConfigNode("Material");
        ConfigNode PQSNode = new ConfigNode("PQS");
        ConfigNode PQSModsNode = new ConfigNode("Mods");
        ConfigNode PQSModsVertexHeightNoiseNode = new ConfigNode("VertexHeightNoise");

        public static void CreatePlanet()
        {
            Planet planet = new Planet();
            planet.Body();
            planet.Template();
            planet.Orbit();
            planet.ScaledVersion();
            planet.PQSMods();
            planet.SavePlanet();
        }

        public void Body()
        {
            body.Name = "Test Planet";
            body.CacheFile = @"Infinity/StarSystems/Cache/Sun/TestPlanet.bin";

            BodyNode.AddValue("name", body.Name);
            BodyNode.AddValue("cacheFile", body.CacheFile);
        }

        public void Template()
        {  
            template.Name = Enums.Template.Tylo;
            template.removeAllPQSMods = false;
            template.removeOcean = true;

            TemplateNode.AddValue("name", template.Name.ToString());
            TemplateNode.AddValue("removeAllPQSMods", template.removeAllPQSMods.ToString());
        }

        public void Orbit()
        {
            orbit.ReferenceBody = "Sun";
            orbit.Inclination = 2;
            orbit.Eccentricity = 0.1;
            orbit.SemiMajorAxis = 2000000000;
            orbit.LongitudeOfAscendingNode = 250;
            orbit.ArgumentOfPeriapsis = 100;
            orbit.Epoch = 5000;
            orbit.meanAnomalyAtEpoch = -2;
            orbit.Color = "#7160C1";

            OrbitNode.AddValue("referenceBody", "NewSun");
            OrbitNode.AddValue("inclination", orbit.Inclination.ToString());
            OrbitNode.AddValue("eccentricity", orbit.Eccentricity.ToString());
            OrbitNode.AddValue("semiMajorAxis", orbit.SemiMajorAxis.ToString());
            OrbitNode.AddValue("longitudeOfAscendingNode", orbit.LongitudeOfAscendingNode.ToString());
            OrbitNode.AddValue("argumentOfPeriapsis", orbit.ArgumentOfPeriapsis.ToString());
            OrbitNode.AddValue("epoch", orbit.Epoch.ToString());
            OrbitNode.AddValue("meanAnomalyAtEpoch", orbit.meanAnomalyAtEpoch.ToString());
            OrbitNode.AddValue("color", orbit.Color);
        }

        public void ScaledVersion()
        {
            scaledVersion.Type = Enums.Body.ScaledVersionTypes.Vacuum;
            scaledVersion.Texture = @"Infinity\StarSystems\Planets\Test_Texture.png";
            scaledVersion.Normals = @"Infinity\StarSystems\Planets\Test_Normals.png";

            ScaledVersionNode.AddValue("Type", scaledVersion.Type.ToString());
            ScaledVersionMaterialNode.AddValue("texture", scaledVersion.Texture);
            ScaledVersionMaterialNode.AddValue("normals", scaledVersion.Normals);
        }

        public void PQSMods()
        {
            

            vhn.deformity = 1000;
            vhn.frequency = 4;
            vhn.octaves = 1;
            vhn.persistence = 1;
            vhn.seed = 716;
            vhn.noiseType = Datas.PQSMods.Enums.noiseType.Perlin;
            vhn.mode = Datas.PQSMods.Enums.mode.Low;
            vhn.lacunarity = 1;
            vhn.order = 100;
            vhn.enabled = true;
            vhn.name = "TBumpsTR";
            vhn.index = 0;

            PQSModsVertexHeightNoiseNode.AddValue("deformity", vhn.deformity.ToString());
            PQSModsVertexHeightNoiseNode.AddValue("frequency", vhn.frequency.ToString());
            PQSModsVertexHeightNoiseNode.AddValue("octaves", vhn.octaves.ToString());
            PQSModsVertexHeightNoiseNode.AddValue("persistence", vhn.persistence.ToString());
            PQSModsVertexHeightNoiseNode.AddValue("seed", vhn.seed.ToString());
            PQSModsVertexHeightNoiseNode.AddValue("noiseType", vhn.noiseType.ToString());
            PQSModsVertexHeightNoiseNode.AddValue("mode", vhn.mode.ToString());
            PQSModsVertexHeightNoiseNode.AddValue("lacunarity", vhn.lacunarity.ToString());
            PQSModsVertexHeightNoiseNode.AddValue("order", vhn.order.ToString());
            PQSModsVertexHeightNoiseNode.AddValue("enabled", vhn.enabled.ToString());
            PQSModsVertexHeightNoiseNode.AddValue("name", vhn.name);
            PQSModsVertexHeightNoiseNode.AddValue("index", vhn.index.ToString());
        }

        public void SavePlanet()
        {
            ConfigNode wrapper = new ConfigNode();
            wrapper.AddConfigNode(MMNode);
            MMNode.AddConfigNode(BodyNode);
                BodyNode.AddConfigNode(TemplateNode);
                BodyNode.AddConfigNode(OrbitNode);
                BodyNode.AddConfigNode(ScaledVersionNode);
                    ScaledVersionNode.AddConfigNode(ScaledVersionMaterialNode);
                BodyNode.AddConfigNode(PQSNode);
                    PQSNode.AddConfigNode(PQSModsNode);
            PQSModsNode.AddConfigNode(PQSModsVertexHeightNoiseNode);
            wrapper.Save(@"F:\Jeux\Non Steam\Kerbal Space Program\Jeux\Modding Planetes\GameData\Infinity\StarSystems\Planets\Test.cfg");
        }
    }
}