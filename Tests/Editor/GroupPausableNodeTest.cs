using Jukey17Games.Utilities.Pausable;
using NUnit.Framework;

namespace Jukey17games.Utilities.Pausable.Tests.Editor
{
    [TestFixture]
    internal sealed class GroupPausableNodeTest
    {
        [Test]
        public void CanBePauseAndResume()
        {
            IGroupPausableSystem system = new GroupPausableSystem();
            var pausableA = new PausableA();
            var nodeA = system.Register(pausableA, Groups.One);
            Assert.That(nodeA.IsPausing, Is.False, "node is not pausing.");
            Assert.That(pausableA.IsPausing, Is.False, "pausable is not pausing.");
            nodeA.Pause();
            Assert.That(nodeA.IsPausing, Is.True, "node is pausing.");
            Assert.That(pausableA.IsPausing, Is.True, "pausable is pausing.");
            nodeA.Resume();
            Assert.That(nodeA.IsPausing, Is.False, "node is not pausing.");
            Assert.That(pausableA.IsPausing, Is.False, "pausable is not pausing.");
        }
    }
}
