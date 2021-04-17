using Moq;
using NUnit.Framework;
using Its.Iasc.Options;

namespace Its.Iasc.Actions
{
    public class UtilsActionTest
    {
        public delegate void ActionFunc(BaseOptions opt);

        [SetUp]
        public void Setup()
        {
        }

        private int RunAction(ActionType type, ActionFunc func, int valueNeed)
        {
            var act = new Mock<IAction>();
            act.Setup(x=>x.Run(It.IsAny<BaseOptions>())).Returns(valueNeed);
            act.Setup(x=>x.GetLastRunStatus()).Returns(valueNeed);

            var opt = new Mock<BaseOptions>();

            UtilsAction.SetAction(type, act.Object);
            func(opt.Object);

            return act.Object.GetLastRunStatus();
        }

        [Test]
        public void RunInitActionTest()
        {
            int valueNeed = 9999;
            int valueReturned = RunAction(ActionType.Init, UtilsAction.RunInitAction, valueNeed);

            Assert.AreEqual(valueNeed, valueReturned, "Last run status not match!!!");
        }

        [Test]
        public void RunPlanActionTest()
        {
            int valueNeed = 9999;
            int valueReturned = RunAction(ActionType.Plan, UtilsAction.RunPlanAction, valueNeed);

            Assert.AreEqual(valueNeed, valueReturned, "Last run status not match!!!");
        }  

        [Test]
        public void RunApplyActionTest()
        {
            int valueNeed = 9999;
            int valueReturned = RunAction(ActionType.Apply, UtilsAction.RunApplyAction, valueNeed);

            Assert.AreEqual(valueNeed, valueReturned, "Last run status not match!!!");
        } 

        [Test]
        public void RunInfoActionTest()
        {
            int valueNeed = 9999;
            int valueReturned = RunAction(ActionType.Info, UtilsAction.RunInfoAction, valueNeed);

            Assert.AreEqual(valueNeed, valueReturned, "Last run status not match!!!");
        }                       
    }
}