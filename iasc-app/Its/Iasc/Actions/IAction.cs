using System;
using Its.Iasc.Options;
using Its.Iasc.Workflows;

namespace Its.Iasc.Actions
{
    public interface IAction
    {
        void SetWorkflow(IWorkflow workflow);
        int Run(BaseOptions options);
        int GetLastRunStatus();
    }
}