using NUnit.Framework;
using Its.Iasc.Options;

namespace Its.Iasc.Actions
{
    public class ActionInfoTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void RunActionTest()
        {
            var opt = new InfoOptions();
            var act = new ActionInfo();

            int status = act.Run(opt);
            int lastRunStatus = act.GetLastRunStatus();

            Assert.AreEqual(status, lastRunStatus, "Status need to be the same!!!");
        }
    }
}