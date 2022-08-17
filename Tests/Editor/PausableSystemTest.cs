using Jukey17Games.Utilities.Pausable;
using NUnit.Framework;

namespace Jukey17games.Utilities.Pausable.Tests.Editor
{
    [TestFixture]
    internal sealed class PausableSystemTest
    {
        [Test]
        public void CanBeRegisteredAndUnregistered()
        {
            IPausableSystem system = new PausableSystem();
            Assert.That(system.Count, Is.Zero, "Nothing registered in system.");
            var pausableA = new PausableA();
            var registered = system.Register(pausableA);
            Assert.That(registered, Is.Not.Null, "Register method result is true.");
            Assert.That(system.Count, Is.Positive, "Any pausable is registered in system.");
            Assert.That(system.Contains(pausableA), Is.True, "PausableA is registered in system.");
            var unregistered = system.Unregister(pausableA);
            Assert.That(unregistered, Is.True, "Unregister method result is true.");
            Assert.That(system.Count, Is.Zero, "Nothing registered in system.");
            Assert.That(system.Contains(pausableA), Is.False, "PausableA is not registered in system.");
        }

        [Test]
        public void CanBeCleared()
        {
            IPausableSystem system = new PausableSystem();
            Assert.That(system.Count, Is.Zero, "Nothing registered in system.");
            var pausableA = new PausableA();
            system.Register(pausableA);
            var pausableB = new PausableB();
            system.Register(pausableB);
            var pausableC = new PausableC();
            system.Register(pausableC);
            Assert.That(system.Count, Is.Positive, "Any pausable is registered in system.");
            system.Clear();
            Assert.That(system.Count, Is.Zero, "Nothing registered in system.");
        }

        [Test]
        public void CanBePauseAndResume()
        {
            IPausableSystem system = new PausableSystem();
            Assert.That(system.Count, Is.Zero, "Nothing registered in system.");
            var pausableA = new PausableA();
            system.Register(pausableA);
            var pausableB = new PausableB();
            system.Register(pausableB);
            var pausableC = new PausableC();
            system.Register(pausableC);
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
    }
}
