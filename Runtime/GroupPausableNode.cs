namespace Jukey17Games.Utilities.Pausable
{
    public interface IGroupPausableNode : IPausableNode
    {
        int Group { get; }

        void AddGroup(int group);
        void RemoveGroup(int group);
        void ChangeGroup(int group);
        bool HasGroup(int group);
    }

    public sealed class GroupPausableNode : IGroupPausableNode
    {
        private readonly IGroupPausableSystem _system;

        internal GroupPausableNode(IGroupPausableSystem system, IPausable pausable)
        {
            _system = system;
            Pausable = pausable;
        }

        public bool IsPausing { get; private set; }
        public IPausable Pausable { get; }
        public int Group { get; private set; }

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

        public void AddGroup(int group)
        {
            Group |= group;
        }

        public void RemoveGroup(int group)
        {
            Group &= ~group;
        }

        public void ChangeGroup(int group)
        {
            Group = group;
        }

        public bool HasGroup(int group)
        {
            return (Group & group) != 0;
        }

        public void Dispose()
        {
            _system.Unregister(this);
        }
    }
}
