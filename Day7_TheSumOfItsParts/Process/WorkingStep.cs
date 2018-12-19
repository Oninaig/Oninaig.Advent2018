using System;
using System.Timers;

namespace Day7_TheSumOfItsParts.Process
{
    public class WorkingStep : Step
    {
        private WorkingStepParams _workingParams;
        public WorkingStep()
        {
        }
        public WorkingStep(WorkingStepParams workParams)
        {
            this._workingParams = workParams;
        }
        public WorkingStep(Step baseStep) : base(baseStep)
        {
        }

        public int RemainingWorkRequired { get; private set; }
        public int WorkRequired { get; private set; }
        

        public WorkingStep Init()
        {
            RemainingWorkRequired = WorkRequired = 0;
            if (_workingParams != null && _workingParams.Debug)
                RemainingWorkRequired = WorkRequired = _workingParams.WorkRequiredOverride;
            else
                RemainingWorkRequired = WorkRequired = StepProcessor.GetWorkTimeForLetter(StepName);
            return this;
        }

        /// <summary>
        ///     Decrement the work counter. If there is no more work do to, return false.
        /// </summary>
        /// <returns></returns>
        public bool DoWork()
        {
            if (--RemainingWorkRequired > 0)
                return true;
            return false;
        }

        public string RemaingWorkPct()
        {
            return
                $"{RemainingWorkRequired} ({Math.Round((((double) WorkRequired - (double) RemainingWorkRequired) / (double) WorkRequired) * 100)}% complete)";
        }

    }

    public class WorkingStepParams
    {
        public bool Debug { get; set; }
        public int WorkRequiredOverride { get; set; }

        public WorkingStepParams(bool debug, int workRequiredOverride)
        {
            Debug = debug;
            WorkRequiredOverride = workRequiredOverride;
        }
    }
}