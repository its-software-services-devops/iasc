
namespace Its.Iasc.Workflows.Utils
{
    public interface ICommandExecutor
    {
        string Exec(string cmd, string argv);
    }
}