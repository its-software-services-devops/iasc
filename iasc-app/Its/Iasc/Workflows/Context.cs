using System;
namespace Its.Iasc.Workflows
{
    public class Context
    {
        public string SourceDir { get; set; }
        public string WipDir { get; set; }
        public string TmpDir { get; set; }
        public string VcsMode { get; set; } //Git, Local
        public string VcsUrl { get; set; } //Git url
        public string VcsFolder { get; set; }
        public string VcsRef { get; set; } //Tag, branch

        public Context()
        {
            SourceDir = Environment.GetEnvironmentVariable("IASC_SRC_DIR");
            WipDir = Environment.GetEnvironmentVariable("IASC_WIP_DIR");
            TmpDir = Environment.GetEnvironmentVariable("IASC_TMP_DIR");
            VcsMode = Environment.GetEnvironmentVariable("IASC_VCS_MODE");
            VcsUrl = Environment.GetEnvironmentVariable("IASC_VCS_URL");
            VcsFolder = Environment.GetEnvironmentVariable("IASC_VCS_FOLDER");
            VcsRef = Environment.GetEnvironmentVariable("IASC_VCS_REF");
        }
    }    
}

