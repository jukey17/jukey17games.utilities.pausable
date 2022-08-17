using System.Collections;
using Jukey17Games.Utilities.Pausable;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

namespace Jukey17games.Utilities.Pausable.Tests.Runtime
{
    internal sealed class ParticleSystemPauseSwitcherTest
    {
        [UnityTest]
        public IEnumerator CanBePauseAndResume()
        {
            var go = Utils.CreatePrimitive(PrimitiveType.Cube);
            var particleSystem = go.AddComponent<ParticleSystem>();
            IParticleSystemPauseSwitcher switcher = go.AddComponent<ParticleSystemPauseSwitcher>();

            switcher.Pause();
            yield return new WaitForSeconds(1.0f);
            Assert.That(particleSystem.isPaused, Is.True, "particle system is paused.");
            Assert.That(switcher.IsPausing, Is.True, "switcher is paused.");
            switcher.Resume();
            Assert.That(particleSystem.isPaused, Is.False, "particle system is resumed.");
            Assert.That(switcher.IsPausing, Is.False, "switcher is resumed.");
        }
    }
}
