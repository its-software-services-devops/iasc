namespace Its.Iasc.Workflows.Models
{
    public class CmdParam
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool NoValue { get; set; }

        public CmdParam()
        {
            NoValue = false;
        }         
    } 
}

