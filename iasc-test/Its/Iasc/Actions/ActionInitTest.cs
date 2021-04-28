using Moq;
using NUnit.Framework;
using Its.Iasc.Options;
using Its.Iasc.Workflows;

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
            var mck = new Mock<IWorkflow>();
            mck.Setup(x=>x.Transform()).Returns(0);
            mck.Setup(x=>x.GetContext()).Returns(new Context());
            mck.Setup(x=>x.LoadFile(It.IsAny<string>())).Returns(0);
            var wf = mck.Object;

            var opt = new InitOptions();
            
            var act = new ActionInit();
            act.SetWorkflow(wf);

            int status = act.Run(opt);
            int lastRunStatus = act.GetLastRunStatus();

            Assert.AreEqual(status, lastRunStatus, "Status need to be the same!!!");
        }          
    }
}