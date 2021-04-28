
namespace Its.Iasc.Workflows
{
    public interface IWorkflow
    {
        int Load(string manifestContent);
        int LoadFile(string fileName);
        int Transform();
        
        Context GetContext();
        // In the future will we will add Init(), Plan() and Apply()
    }
}