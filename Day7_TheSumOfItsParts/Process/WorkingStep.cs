namespace Day7_TheSumOfItsParts.Process
{
    public class WorkingStep : Step
    {
        public WorkingStep()
        {
        }

        public WorkingStep(Step baseStep) : base(baseStep)
        {
        }

        public int WorkRequired { get; private set; }

        public WorkingStep Init()
        {
            WorkRequired = StepProcessor.GetWorkTimeForLetter(StepName);
            return this;
        }

        /// <summary>
        ///     Decrement the work counter. If there is no more work do to, return false.
        /// </summary>
        /// <returns></returns>
        public bool DoWork()
        {
            if (--WorkRequired > 0)
                return true;
            return false;
        }
    }
}