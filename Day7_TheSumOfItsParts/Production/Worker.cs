using Day7_TheSumOfItsParts.Process;

namespace Day7_TheSumOfItsParts.Production
{
    public class Worker
    {
        public Worker(int id)
        {
            Id = id;
        }

        public int Id { get; }
        public WorkingStep CurrWorkStep { get; private set; }

        public bool IsBusy => CurrWorkStep.WorkRequired > 0;

        public void SetWork(WorkingStep workStep)
        {
            CurrWorkStep = workStep;
            CurrWorkStep.Init();
        }
    }
}