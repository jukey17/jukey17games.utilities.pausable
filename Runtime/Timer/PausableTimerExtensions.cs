using System;

namespace Jukey17Games.Utilities.Pausable
{
    public static class PausableTimerExtensions
    {
        public static IPausableTimer AddOnStarted(this IPausableTimer self, Action<PausableTimerEventArgs> action)
        {
            self.AddListener(new PausableTimerListener(action));
            return self;
        }

        public static IPausableTimer AddOnStopped(this IPausableTimer self, Action<PausableTimerEventArgs> action)
        {
            self.AddListener(new PausableTimerListener(onStopped: action));
            return self;
        }

        public static IPausableTimer AddOnPaused(this IPausableTimer self, Action<PausableTimerEventArgs> action)
        {
            self.AddListener(new PausableTimerListener(onPaused: action));
            return self;
        }

        public static IPausableTimer AddOnResumed(this IPausableTimer self, Action<PausableTimerEventArgs> action)
        {
            self.AddListener(new PausableTimerListener(onResumed: action));
            return self;
        }

        public static IPausableTimer AddOnElapsed(this IPausableTimer self, Action<PausableTimerEventArgs> action)
        {
            self.AddListener(new PausableTimerListener(onElapsed: action));
            return self;
        }
    }
}
