namespace Its.Iasc.Workflows.Utils
{
    public class UtilsCommandExecutor : ICommandExecutor
    {
        public string Exec(string cmd, string argv)
        {
            return Utils.Exec(cmd, argv);
        }
    } 
}

