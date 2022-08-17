using System.Collections;
using Jukey17Games.Utilities.Pausable;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

namespace Jukey17games.Utilities.Pausable.Tests.Runtime
{
    internal sealed class AnimatorPauseSwitcherTest
    {
        [UnityTest]
        public IEnumerator CanBePauseAndResume()
        {
            var comparer = FloatEqualityComparer.Instance;

            var go = Utils.CreatePrimitive(PrimitiveType.Cube);
            var animator = go.AddComponent<Animator>();
            IAnimatorPauseSwitcher switcher = go.AddComponent<AnimatorPauseSwitcher>();

            switcher.Pause();
            yield return new WaitForSeconds(1.0f);
            Assert.That(animator.speed, Is.EqualTo(0.0f).Using(comparer), "animator speed is zero.");
            Assert.That(switcher.IsPausing, Is.True, "switcher is paused.");
            switcher.Resume();
            Assert.That(animator.speed, Is.EqualTo(1.0f).Using(comparer), "animator speed is one.");
            Assert.That(switcher.IsPausing, Is.False, "switcher is resumed.");
        }
    }
}
