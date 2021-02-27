using System;
using Its.Iasc.Options;

namespace Its.Iasc.Actions
{
    public interface IAction
    {
        int Run(BaseOptions options);
        int GetLastRunStatus();
    }
}