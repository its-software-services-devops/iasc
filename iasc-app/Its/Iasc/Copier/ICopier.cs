using Its.Iasc.Workflows.Models;

namespace Its.Iasc.Copier
{
    public interface ICopier
    {
        void SetWipDir(string wpDir);
        void Process(CopyItem[] copyItems);
    }
}