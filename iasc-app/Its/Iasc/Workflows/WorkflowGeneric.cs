
using Its.Iasc.Workflows.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Its.Iasc.Workflows
{
    public class WorkflowGeneric : WorkflowBase
    {
        private IDeserializer deserializer = null;

        public WorkflowGeneric()
        {
            deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
        }

        protected override Manifest ParseManifest(string manifestContent)
        {
            return deserializer.Deserialize<Manifest>(manifestContent);
        }
    }
}