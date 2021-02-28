using NUnit.Framework;
using Its.Iasc.Options;

namespace Its.Iasc.Actions
{
    public class ActionApplyTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void RunActionTest()
        {
            var opt = new ApplyOptions();
            var act = new ActionApply();

            int status = act.Run(opt);
            int lastRunStatus = act.GetLastRunStatus();

            Assert.AreEqual(status, lastRunStatus, "Status need to be the same!!!");
        }          
    }
}