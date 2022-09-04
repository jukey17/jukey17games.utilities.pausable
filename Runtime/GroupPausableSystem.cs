using System;
using System.Collections.Generic;
using System.Linq;

namespace Jukey17Games.Utilities.Pausable
{
    public interface IGroupPausableSystem : IPauseSwitcher, IDisposable
    {
        int Count { get; }
        IGroupPausableNode Register(IPausable pausable, int group);
        bool Unregister(IPausable pausable);
        bool Unregister(IGroupPausableNode node);
        bool Contains(IPausable pausable);
        bool Contains(IGroupPausableNode node);
        void Clear();
        void Pause(int group);
        void Resume(int group);
    }

    public sealed class GroupPausableSystem : IGroupPausableSystem
    {
        private readonly LinkedList<IGroupPausableNode> _nodes = new LinkedList<IGroupPausableNode>();
        private bool _disposed;

        public bool IsPausing => _nodes.All(pausable => pausable.IsPausing);
        public int Count => _nodes.Count;

        public IGroupPausableNode Register(IPausable pausable, int group)
        {
            if (_disposed)
            {
                return null;
            }

            var node = _nodes.FirstOrDefault(node => node.Pausable == pausable) ??
                       new GroupPausableNode(this, pausable);
            node.AddGroup(group);
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

        public bool Unregister(IGroupPausableNode node)
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

        public bool Contains(IGroupPausableNode node)
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

            foreach (var pausable in _nodes)
            {
                pausable.Pause();
            }
        }

        public void Pause(int group)
        {
            if (_disposed)
            {
                return;
            }

            foreach (var pausable in _nodes.Where(pausable => pausable.HasGroup(group)))
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

            foreach (var pausable in _nodes)
            {
                pausable.Resume();
            }
        }

        public void Resume(int group)
        {
            if (_disposed)
            {
                return;
            }

            foreach (var pausable in _nodes.Where(pausable => pausable.HasGroup(group)))
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
            _nodes.Clear();
        }
    }
}
