using Its.Iasc.Workflows.Models;

namespace Its.Iasc.Copier
{
    public class GenericCopier : ICopier
    {
        private string wipDir = "";

        public void Process(CopyItem[] copyItems)
        {
        }

        public void SetWipDir(string wpDir)
        {
            wipDir = wpDir;
        }
    }
}