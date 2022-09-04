using Jukey17Games.Utilities.Pausable;
using NUnit.Framework;

namespace Jukey17games.Utilities.Pausable.Tests.Editor
{
    [TestFixture]
    internal sealed class PausableNodeTest
    {
        [Test]
        public void CanBePauseAndResume()
        {
            IPausableSystem system = new PausableSystem();
            var pausable = new PausableA();
            var node = system.Register(pausable);
            Assert.That(node.IsPausing, Is.False, "node is not pausing.");
            Assert.That(pausable.IsPausing, Is.False, "pausable is not pausing.");
            node.Pause();
            Assert.That(node.IsPausing, Is.True, "node is pausing.");
            Assert.That(pausable.IsPausing, Is.True, "pausable is pausing.");
            node.Resume();
            Assert.That(node.IsPausing, Is.False, "node is not pausing.");
            Assert.That(pausable.IsPausing, Is.False, "pausable is not pausing.");
        }
    }
}
