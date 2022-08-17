using System.Collections;
using Jukey17Games.Utilities.Pausable;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

// ReSharper disable Unity.InefficientPropertyAccess

namespace Jukey17games.Utilities.Pausable.Tests.Runtime
{
    internal sealed class RigidbodyPauseSwitcherTest
    {
        [UnityTest]
        public IEnumerator CanBePauseAndResume()
        {
            var comparer = Vector3EqualityComparer.Instance;

            var go = Utils.CreatePrimitive(PrimitiveType.Cube);
            var rigidbody = go.AddComponent<Rigidbody>();
            IRigidbodyPauseSwitcher switcher = go.AddComponent<RigidbodyPauseSwitcher>();

            switcher.Pause();

            var position1 = go.transform.position;
            yield return new WaitForSeconds(1.0f);
            var position2 = go.transform.position;
            Assert.That(position1, Is.EqualTo(position2).Using(comparer), "positions are same values.");
            Assert.That(rigidbody.isKinematic, Is.True, "rigidbody is kinematic.");
            Assert.That(switcher.IsPausing, Is.True, "switcher is paused.");

            switcher.Resume();

            yield return new WaitForSeconds(1.0f);
            var position3 = go.transform.position;
            Assert.That(position2, Is.Not.EqualTo(position3).Using(comparer), "positions are different values.");
            Assert.That(rigidbody.isKinematic, Is.False, "rigidbody is not kinematic.");
            Assert.That(switcher.IsPausing, Is.False, "switcher is resumed.");
        }
    }
}
