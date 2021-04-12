using NUnit.Framework;
using Its.Iasc.Options;

namespace Its.Iasc.Actions
{
    public class ActionPlanTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void RunActionTest()
        {
            var opt = new PlanOptions();
            var act = new ActionPlan();

            int status = act.Run(opt);
            int lastRunStatus = act.GetLastRunStatus();

            Assert.AreEqual(status, lastRunStatus, "Status need to be the same!!!");
        }          
    }
}