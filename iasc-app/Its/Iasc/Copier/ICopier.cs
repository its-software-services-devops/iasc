using Its.Iasc.Workflows.Models;

namespace Its.Iasc.Copier
{
    public enum CopyType 
    {
        GsUtilCp,
        Http,
        Cp
    }

    public interface ICopier
    {
        void SetSrcDir(string sDir);
        void SetWipDir(string wpDir);
        void Process(CopyItem[] copyItems);
        void SetCopyCmd(CopyType cpType, string cmd);
    }
}