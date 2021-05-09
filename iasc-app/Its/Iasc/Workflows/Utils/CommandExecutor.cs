namespace Its.Iasc.Workflows.Utils
{
    public class CommandExecutor : ICommandExecutor
    {
        public string Exec(string cmd, string argv)
        {
            return Utils.Exec(cmd, argv);
        }
    } 
}

