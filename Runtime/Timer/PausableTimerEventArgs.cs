namespace Jukey17Games.Utilities.Pausable
{
    public readonly struct PausableTimerEventArgs
    {
        public PausableTimerEventArgs(long interval, long elapsed, int count, bool oneShot)
        {
            Interval = interval;
            Elapsed = elapsed;
            Count = count;
            OneShot = oneShot;
        }

        public readonly long Interval;
        public readonly long Elapsed;
        public readonly int Count;
        public readonly bool OneShot;
    }
}
