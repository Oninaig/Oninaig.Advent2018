using System;
using System.Linq;
using System.Timers;
using Day7_TheSumOfItsParts.Process.Helpers;

namespace Day7_TheSumOfItsParts.Process
{
    public class WorkingStep : Step, IEquatable<WorkingStep>
    {
        private WorkingStepParams _workingParams;
        private bool _initialized;
    
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

        public override int GetHashCode()
        {
            return this.UniqueStepId.GetHashCode();
        }
        public bool Equals(WorkingStep other)
        {
            if (other is null)
                return false;
            if (this.UniqueStepId == other.UniqueStepId)
                return true;
            return false;
        }

        public override bool Equals(object obj) => Equals(obj as WorkingStep);

        public int RemainingWorkRequired { get; set; }
        public int WorkRequired { get; private set; }

        public bool HasPrerequisites =>
            this.PreRequisites.Any(x => !x.Key.IsCompleted);

        public WorkingStep Init()
        {
            if (!_initialized)
            {
                RemainingWorkRequired = WorkRequired = 0;
                if (_workingParams != null && _workingParams.Debug)
                    RemainingWorkRequired = WorkRequired = _workingParams.WorkRequiredOverride;
                else
                    RemainingWorkRequired = WorkRequired = StepProcessor.GetWorkTimeForLetter(StepName);
                _initialized = true;
            }
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

        public string Identify()
        {
            return $"{this.StepName}-{this.RemainingWorkRequired} / {this.WorkRequired}";
        }
        public string IdentifyRemaining()
        {
            return $"{this.StepName} - {this.RemainingWorkRequired}";
        }

        public override object AddPrerequisite(Step requiredStep)
        {
            PreRequisites.Add(requiredStep, false);
            return this;
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