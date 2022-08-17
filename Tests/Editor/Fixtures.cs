using System;
using Jukey17Games.Utilities.Pausable;

namespace Jukey17games.Utilities.Pausable.Tests.Editor
{
    internal abstract class PausableBase : IPausable
    {
        public bool IsPausing { get; private set; }

        public void OnPaused()
        {
            IsPausing = true;
        }

        public void OnResumed()
        {
            IsPausing = false;
        }
    }

    internal sealed class PausableA : PausableBase
    {
    }

    internal sealed class PausableB : PausableBase
    {
    }

    internal sealed class PausableC : PausableBase
    {
    }

    internal sealed class PausableD : PausableBase
    {
    }

    internal sealed class PausableTimerListener : IPausableTimerListener
    {
        public bool CallOnStarted { get; private set; }
        public bool CallOnStopped { get; private set; }
        public bool CallOnPaused { get; private set; }
        public bool CallOnResumed { get; private set; }
        public bool CallOnElapsed { get; private set; }

        public void OnStarted(in PausableTimerEventArgs args)
        {
            CallOnStarted = true;
        }

        public void OnStopped(in PausableTimerEventArgs args)
        {
            CallOnStopped = true;
        }

        public void OnPaused(in PausableTimerEventArgs args)
        {
            CallOnPaused = true;
        }

        public void OnResumed(in PausableTimerEventArgs args)
        {
            CallOnResumed = true;
        }

        public void OnElapsed(in PausableTimerEventArgs args)
        {
            CallOnElapsed = true;
        }

        public void Reset()
        {
            CallOnStarted = CallOnStopped = CallOnPaused = CallOnResumed = CallOnElapsed = false;
        }
    }

    [Flags]
    internal enum Groups
    {
        None = 0,
        One = 1 << 0,
        Two = 1 << 1,
        Three = 1 << 2,
    }

    internal static class GroupsExtensions
    {
        public static IGroupPausableNode Register(this IGroupPausableSystem self, IPausable pausable, Groups groups)
        {
            return self.Register(pausable, (int) groups);
        }

        public static void Pause(this IGroupPausableSystem self, Groups groups)
        {
            self.Pause((int) groups);
        }

        public static void Resume(this IGroupPausableSystem self, Groups groups)
        {
            self.Resume((int) groups);
        }

        public static void AddGroup(this IGroupPausableNode self, Groups groups)
        {
            self.AddGroup((int) groups);
        }

        public static void RemoveGroup(this IGroupPausableNode self, Groups groups)
        {
            self.RemoveGroup((int) groups);
        }

        public static void ChangeGroup(this IGroupPausableNode self, Groups groups)
        {
            self.ChangeGroup((int) groups);
        }

        public static bool HasGroup(this IGroupPausableNode self, Groups groups)
        {
            return self.HasGroup((int) groups);
        }
    }
}
