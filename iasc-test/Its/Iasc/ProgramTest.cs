using Moq;
using NUnit.Framework;
using Its.Iasc.Options;
using Its.Iasc.Actions;

namespace Its.Iasc
{
    public class ProgramTest
    {
        public delegate void ActionFunc(BaseOptions opt);

        [SetUp]
        public void Setup()
        {
        }

        private IAction CreateMockedAction(int valueNeed)
        {
            var act = new Mock<IAction>();
            act.Setup(x=>x.Run(It.IsAny<BaseOptions>())).Returns(valueNeed);
            act.Setup(x=>x.GetLastRunStatus()).Returns(valueNeed);

            return act.Object;
        }

        [Test]
        public void MainTest()
        {
            int initValueNeed = 9999;
            IAction initAct = CreateMockedAction(initValueNeed);
            
            int planValueNeed = 9998;
            IAction planAct = CreateMockedAction(planValueNeed);

            int applyValueNeed = 9997;
            IAction applyAct = CreateMockedAction(applyValueNeed);

            UtilsAction.SetAction(ActionType.Init, initAct);
            UtilsAction.SetAction(ActionType.Plan, planAct);
            UtilsAction.SetAction(ActionType.Apply, applyAct);

            string[] args = {"init", "-v", "hello"}; 
            Program.Main(args);

            Assert.AreEqual(initValueNeed, initAct.GetLastRunStatus(), "Last run status not match!!!");
            Assert.AreEqual(planValueNeed, planAct.GetLastRunStatus(), "Last run status not match!!!");
            Assert.AreEqual(applyValueNeed, applyAct.GetLastRunStatus(), "Last run status not match!!!");
        }            
    }
}