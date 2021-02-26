using Its.Iasc.Options;

namespace Its.Iasc.Actions
{
    public abstract class BaseAction : IAction
    {
        protected abstract int RunAction(BaseOptions options);

        public int Run(BaseOptions options)
        {
            return RunAction(options);
        }
    }
}