using Jukey17Games.Utilities.Pausable;
using NUnit.Framework;

namespace Jukey17games.Utilities.Pausable.Tests.Editor
{
    [TestFixture]
    internal sealed class GroupPausableSystemTest
    {
        [Test]
        public void CanBeRegisteredAndUnregistered()
        {
            IGroupPausableSystem system = new GroupPausableSystem();
            Assert.That(system.Count, Is.Zero, "Nothing registered in system.");
            var pausableA = new PausableA();
            var registered = system.Register(pausableA, Groups.One);
            Assert.That(registered, Is.Not.Null, "Register method result is true.");
            Assert.That(system.Count, Is.Positive, "Any pausable is registered in system.");
            Assert.That(system.Contains(pausableA), Is.True, "PausableA is registered in system.");
            var unregistered = system.Unregister(pausableA);
            Assert.That(unregistered, Is.True, "Unregister method result is true.");
            Assert.That(system.Count, Is.Zero, "Nothing registered in system.");
            Assert.That(system.Contains(pausableA), Is.False, "PausableA is not registered in system.");
        }

        [Test]
        public void CanBeRegisteredInMultipleGroups()
        {
            IGroupPausableSystem system = new GroupPausableSystem();
            Assert.That(system.Count, Is.Zero, "Nothing registered in system.");
            var pausableA = new PausableA();
            var first = system.Register(pausableA, Groups.One);
            Assert.That(first, Is.Not.Null, "Register method result is true.");
            var second = system.Register(pausableA, Groups.Two);
            Assert.That(second, Is.EqualTo(first), "The first and second are the same instance.");
            Assert.That(system.Count, Is.Positive, "Any pausable is registered in system.");
            Assert.That(system.Contains(pausableA), Is.True, "PausableA is registered in system.");
        }

        [Test]
        public void CanBeCleared()
        {
            IGroupPausableSystem system = new GroupPausableSystem();
            Assert.That(system.Count, Is.Zero, "Nothing registered in system.");
            var pausableA = new PausableA();
            system.Register(pausableA, Groups.One);
            var pausableB = new PausableB();
            system.Register(pausableB, Groups.Two);
            var pausableC = new PausableC();
            system.Register(pausableC, Groups.Three);
            Assert.That(system.Count, Is.Positive, "Any pausable is registered in system.");
            system.Clear();
            Assert.That(system.Count, Is.Zero, "Nothing registered in system.");
        }

        [Test]
        public void CanBePauseAndResume()
        {
            IGroupPausableSystem system = new GroupPausableSystem();
            Assert.That(system.Count, Is.Zero, "Nothing registered in system.");
            var pausableA = new PausableA();
            system.Register(pausableA, Groups.One);
            var pausableB = new PausableB();
            system.Register(pausableB, Groups.Two);
            var pausableC = new PausableC();
            system.Register(pausableC, Groups.Three);
            system.Pause();
            Assert.That(system.IsPausing, Is.True, "System is pausing.");
            Assert.That(pausableA.IsPausing, Is.True, "PausableA is pausing.");
            Assert.That(pausableB.IsPausing, Is.True, "PausableB is pausing.");
            Assert.That(pausableC.IsPausing, Is.True, "PausableC is pausing.");
            system.Resume();
            Assert.That(system.IsPausing, Is.False, "System is not pausing.");
            Assert.That(pausableA.IsPausing, Is.False, "PausableA is not pausing.");
            Assert.That(pausableB.IsPausing, Is.False, "PausableB is not pausing.");
            Assert.That(pausableC.IsPausing, Is.False, "PausableC is not pausing.");
        }

        [Test]
        public void CanBePauseAndResumeWithGroup()
        {
            IGroupPausableSystem system = new GroupPausableSystem();
            Assert.That(system.Count, Is.Zero, "Nothing registered in system.");
            var pausableA = new PausableA();
            system.Register(pausableA, Groups.One);
            var pausableB = new PausableB();
            system.Register(pausableB, Groups.Two);
            var pausableC = new PausableC();
            system.Register(pausableC, Groups.Three);
            var pausableD = new PausableD();
            system.Register(pausableD, Groups.Three);
            system.Pause(Groups.Three);
            Assert.That(system.IsPausing, Is.False, "System is not pausing.");
            Assert.That(pausableA.IsPausing, Is.False, "PausableA is not pausing.");
            Assert.That(pausableB.IsPausing, Is.False, "PausableB is not pausing.");
            Assert.That(pausableC.IsPausing, Is.True, "PausableC is pausing.");
            Assert.That(pausableD.IsPausing, Is.True, "PausableD is pausing.");
            system.Resume(Groups.Three);
            Assert.That(system.IsPausing, Is.False, "System is not pausing.");
            Assert.That(pausableA.IsPausing, Is.False, "PausableA is not pausing.");
            Assert.That(pausableB.IsPausing, Is.False, "PausableB is not pausing.");
            Assert.That(pausableC.IsPausing, Is.False, "PausableC is not pausing.");
            Assert.That(pausableD.IsPausing, Is.False, "PausableD is not pausing.");
        }
    }
}
