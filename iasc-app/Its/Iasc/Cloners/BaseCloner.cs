using Its.Iasc.Workflows;

namespace Its.Iasc.Cloners
{
    public abstract class BaseCloner : ICloner
    {
        protected Context context = null;
        
        public abstract void Clone();

        public BaseCloner()
        {
        }

        public void SetContext(Context ctx)
        {
            context = ctx;
        }
    }
}