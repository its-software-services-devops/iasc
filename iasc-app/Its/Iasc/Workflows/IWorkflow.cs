
namespace Its.Iasc.Workflows
{
    public interface IWorkflow
    {
        int Load(string manifestContent);
        int Transform();
        
        // In the future will we will add Init(), Plan() and Apply()
    }
}