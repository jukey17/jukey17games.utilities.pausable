using System;
using System.Collections.Generic;
using System.Linq;

namespace Jukey17Games.Utilities.Pausable
{
    public interface IPausableSystem : IPauseSwitcher, IDisposable
    {
        int Count { get; }
        IPausableNode Register(IPausable pausable);
        bool Unregister(IPausable pausable);
        bool Unregister(IPausableNode node);
        bool Contains(IPausable pausable);
        bool Contains(IPausableNode node);
        void Clear();
    }

    public sealed class PausableSystem : IPausableSystem
    {
        private readonly LinkedList<IPausableNode> _nodes = new LinkedList<IPausableNode>();
        private bool _disposed;

        public bool IsPausing { get; private set; }
        public int Count => _nodes.Count;

        public IPausableNode Register(IPausable pausable)
        {
            if (_disposed)
            {
                return null;
            }

            var node = _nodes.FirstOrDefault(node => node.Pausable == pausable) ??
                       new PausableNode(this, pausable);
            _nodes.AddLast(node);
            return node;
        }

        public bool Unregister(IPausable pausable)
        {
            if (_disposed)
            {
                return false;
            }

            var node = _nodes.FirstOrDefault(node => node.Pausable == pausable);
            return node != null && _nodes.Remove(node);
        }

        public bool Unregister(IPausableNode node)
        {
            if (_disposed)
            {
                return false;
            }

            return _nodes.Remove(node);
        }

        public bool Contains(IPausable pausable)
        {
            if (_disposed)
            {
                return false;
            }

            return _nodes.FirstOrDefault(node => node.Pausable == pausable) != null;
        }

        public bool Contains(IPausableNode node)
        {
            if (_disposed)
            {
                return false;
            }

            return _nodes.Contains(node);
        }

        public void Clear()
        {
            if (_disposed)
            {
                return;
            }

            _nodes.Clear();
        }

        public void Pause()
        {
            if (_disposed)
            {
                return;
            }

            if (IsPausing)
            {
                return;
            }

            IsPausing = true;
            foreach (var pausable in _nodes)
            {
                pausable.Pause();
            }
        }

        public void Resume()
        {
            if (_disposed)
            {
                return;
            }

            if (!IsPausing)
            {
                return;
            }

            IsPausing = false;
            foreach (var pausable in _nodes)
            {
                pausable.Resume();
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            Clear();
        }
    }
}
