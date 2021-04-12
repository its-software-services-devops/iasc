using NUnit.Framework;
using Its.Iasc.Options;

namespace Its.Iasc.Actions
{
    public class ActionInitTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void RunActionTest()
        {
            var opt = new InitOptions();
            var act = new ActionInit();

            int status = act.Run(opt);
            int lastRunStatus = act.GetLastRunStatus();

            Assert.AreEqual(status, lastRunStatus, "Status need to be the same!!!");
        }          
    }
}