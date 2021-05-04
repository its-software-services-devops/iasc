using Its.Iasc.Workflows;

namespace Its.Iasc.Cloners
{
    public interface ICloner
    {
        void SetContext(Context ctx);
        void Clone();
    }
}