using Its.Iasc.Options;
using Its.Iasc.Workflows;

namespace Its.Iasc.Actions
{
    public abstract class BaseAction : IAction
    {
        private int lastRunStatus = 1;
        private IWorkflow wf = new WorkflowGeneric();

        protected abstract int RunAction(BaseOptions options);

        public void SetWorkflow(IWorkflow workflow)
        {
            wf = workflow;
        }

        protected IWorkflow GetWorkflow()
        {
            return wf;
        }

        public int Run(BaseOptions options)
        {
            lastRunStatus = RunAction(options);
            return lastRunStatus;
        }

        public int GetLastRunStatus()
        {
            return lastRunStatus;
        }
    }
}