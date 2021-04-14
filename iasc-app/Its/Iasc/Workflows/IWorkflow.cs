
namespace Its.Iasc.Workflows
{
    public interface IWorkflow
    {
        int Load(string manifestContent);
        // In the future will we will add Init(), Plan() and Apply()
    }
}