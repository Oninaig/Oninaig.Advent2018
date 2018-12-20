using System;
using System.Linq;
using Day7_TheSumOfItsParts.Process.Helpers;

namespace Day7_TheSumOfItsParts.Process
{
    public class WorkingStep : Step, IEquatable<WorkingStep>
    {
        private readonly WorkingStepParams _workingParams;
        private bool _initialized;

        public WorkingStep()
        {
        }

        public WorkingStep(WorkingStepParams workParams)
        {
            _workingParams = workParams;
        }

        public WorkingStep(Step baseStep) : base(baseStep)
        {
        }

        public int RemainingWorkRequired { get; set; }
        public int WorkRequired { get; private set; }

        public bool HasPrerequisites =>
            PreRequisites.Any(x => !x.Key.IsCompleted);

        public bool Equals(WorkingStep other)
        {
            if (other is null)
                return false;
            if (UniqueStepId == other.UniqueStepId)
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            return UniqueStepId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as WorkingStep);
        }

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
                $"{RemainingWorkRequired} ({Math.Round((WorkRequired - (double) RemainingWorkRequired) / WorkRequired * 100)}% complete)";
        }

        public string Identify()
        {
            return $"{StepName}-{RemainingWorkRequired} / {WorkRequired}";
        }

        public string IdentifyRemaining()
        {
            return $"{StepName} - {RemainingWorkRequired}";
        }

        public override object AddPrerequisite(Step requiredStep)
        {
            PreRequisites.Add(requiredStep, false);
            return this;
        }
    }

    public class WorkingStepParams
    {
        public WorkingStepParams(bool debug, int workRequiredOverride)
        {
            Debug = debug;
            WorkRequiredOverride = workRequiredOverride;
        }

        public bool Debug { get; set; }
        public int WorkRequiredOverride { get; set; }
    }
}