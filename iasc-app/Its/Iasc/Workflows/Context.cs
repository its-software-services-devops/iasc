
namespace Its.Iasc.Workflows
{
    public class Context
    {
        public string SourceDir { get; set; }
        public string WipDir { get; set; }

        public Context()
        {
            SourceDir = ".";
            WipDir = "";
        }
    }    
}

