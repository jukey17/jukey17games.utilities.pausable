using System;

namespace Jukey17Games.Utilities.Pausable
{
    public interface IPausableTimerListener
    {
        void OnStarted(in PausableTimerEventArgs args);
        void OnStopped(in PausableTimerEventArgs args);
        void OnPaused(in PausableTimerEventArgs args);
        void OnResumed(in PausableTimerEventArgs args);
        void OnElapsed(in PausableTimerEventArgs args);
    }

    internal sealed class PausableTimerListener : IPausableTimerListener
    {
        private readonly Action<PausableTimerEventArgs> _onStarted;
        private readonly Action<PausableTimerEventArgs> _onStopped;
        private readonly Action<PausableTimerEventArgs> _onPaused;
        private readonly Action<PausableTimerEventArgs> _onResumed;
        private readonly Action<PausableTimerEventArgs> _onElapsed;

        public PausableTimerListener(Action<PausableTimerEventArgs> onStarted = null,
            Action<PausableTimerEventArgs> onStopped = null,
            Action<PausableTimerEventArgs> onPaused = null,
            Action<PausableTimerEventArgs> onResumed = null,
            Action<PausableTimerEventArgs> onElapsed = null)
        {
            _onStarted = onStarted;
            _onStopped = onStopped;
            _onPaused = onPaused;
            _onResumed = onResumed;
            _onElapsed = onElapsed;
        }

        public void OnStarted(in PausableTimerEventArgs args)
        {
            _onStarted?.Invoke(args);
        }

        public void OnStopped(in PausableTimerEventArgs args)
        {
            _onStopped?.Invoke(args);
        }

        public void OnPaused(in PausableTimerEventArgs args)
        {
            _onPaused?.Invoke(args);
        }

        public void OnResumed(in PausableTimerEventArgs args)
        {
            _onResumed?.Invoke(args);
        }

        public void OnElapsed(in PausableTimerEventArgs args)
        {
            _onElapsed?.Invoke(args);
        }
    }
}
