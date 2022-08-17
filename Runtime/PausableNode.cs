using System;

namespace Jukey17Games.Utilities.Pausable
{
    public interface IPausableNode : IDisposable
    {
        bool IsPausing { get; }
        IPausable Pausable { get; }

        void Pause();
        void Resume();
    }

    public sealed class PausableNode : IPausableNode
    {
        private readonly IPausableSystem _system;

        internal PausableNode(IPausableSystem system, IPausable pausable)
        {
            _system = system;
            Pausable = pausable;
        }

        public bool IsPausing { get; private set; }
        public IPausable Pausable { get; }

        public void Pause()
        {
            if (IsPausing)
            {
                return;
            }

            IsPausing = true;
            Pausable.OnPaused();
        }

        public void Resume()
        {
            if (!IsPausing)
            {
                return;
            }

            IsPausing = false;
            Pausable.OnResumed();
        }

        public void Dispose()
        {
            _system.Unregister(this);
        }
    }
}
