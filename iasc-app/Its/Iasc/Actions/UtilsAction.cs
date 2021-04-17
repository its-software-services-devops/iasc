using Its.Iasc.Options;

namespace Its.Iasc.Actions
{
    public enum ActionType
    {
        Init,
        Plan,
        Apply,
        Info,
    }

    public static class UtilsAction
    {
        private static IAction initAction = new ActionInit();
        private static IAction planAction = new ActionPlan();
        private static IAction applyAction = new ActionApply();
        private static IAction infoAction = new ActionInfo();

        public static void SetAction(ActionType type, IAction action)
        {
            if (type == ActionType.Apply)
            {
                applyAction = action;
            }
            else if (type == ActionType.Plan)
            {
                planAction = action;
            } 
            else if (type == ActionType.Init)
            {
                initAction = action;
            } 
            else if (type == ActionType.Info)
            {
                infoAction = action;
            }             
        }

        public static void RunInitAction(BaseOptions o)
        {
            initAction.Run(o);
        }

        public static void RunPlanAction(BaseOptions o)
        {
            planAction.Run(o);            
        }

        public static void RunApplyAction(BaseOptions o)
        {
            applyAction.Run(o);      
        }

        public static void RunInfoAction(BaseOptions o)
        {
            infoAction.Run(o);
        }        
    }
}
