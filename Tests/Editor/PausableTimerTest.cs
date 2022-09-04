using System.Collections;
using Cysharp.Threading.Tasks;
using Jukey17Games.Utilities.Pausable;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Jukey17games.Utilities.Pausable.Tests.Editor
{
    [TestFixture]
    internal sealed class PausableTimerTest
    {
        [UnityTest]
        public IEnumerator CanBeStartAndStop()
        {
            IPausableTimer timer = new PausableTimer(true, 100);
            var calledOnStarted = false;
            timer.AddOnStarted(_ => calledOnStarted = true);
            var calledOnStopped = false;
            timer.AddOnStopped(_ => calledOnStopped = true);
            timer.Start();
            Assert.That(timer.IsStarting, Is.True, "Timer is starting.");
            Assert.That(calledOnStarted, Is.True, "OnStarted is called.");

            yield return UniTask.ToCoroutine(async () =>
            {
                await UniTask.Delay(50);
                timer.Stop();
                Assert.That(timer.IsStarting, Is.False, "Timer is stopped.");
                Assert.That(calledOnStopped, Is.True, "OnStopped is called.");
            });
        }

        [UnityTest]
        public IEnumerator CanBePauseAndResume()
        {
            IPausableTimer timer = new PausableTimer(true, 100);
            var calledOnPaused = false;
            timer.AddOnPaused(_ => calledOnPaused = true);
            var calledOnResumed = false;
            timer.AddOnResumed(_ => calledOnResumed = true);
            timer.Start();

            yield return UniTask.ToCoroutine(async () =>
            {
                await UniTask.Delay(50);
                timer.Pause();
                Assert.That(timer.IsPausing, Is.True, "Timer is pausing.");
                Assert.That(calledOnPaused, Is.True, "OnPaused is called.");

                await UniTask.Delay(50);
                timer.Resume();
                Assert.That(timer.IsPausing, Is.False, "Timer is resumed.");
                Assert.That(calledOnResumed, Is.True, "OnResumed is called.");
            });
        }

        [UnityTest]
        public IEnumerator CanBeElapsedOneShot()
        {
            yield return UniTask.ToCoroutine(async () =>
            {
                IPausableTimer timer = new PausableTimer(true, 100);
                var callOnElapsed = false;
                timer.AddOnElapsed(_ => callOnElapsed = true);
                await timer.StartAsync();
                Assert.That(timer.IsStarting, Is.False, "Timer is stopped.");
                Assert.That(callOnElapsed, Is.True, "OnElapsed is called.");
            });
        }

        [UnityTest]
        public IEnumerator CanBeElapsedRepeat()
        {
            IPausableTimer timer = new PausableTimer(false, 100);
            var count = 0;
            timer.AddOnElapsed(args => count = args.Count);
            timer.Start();
            yield return UniTask.ToCoroutine(async () =>
            {
                await UniTask.Delay(1010);
                timer.Stop();
                Assert.That(timer.IsStarting, Is.False, "Timer is stopped.");
                Assert.That(count, Is.GreaterThanOrEqualTo(10), "OnElapsed is called.");
            });
        }

        [UnityTest]
        public IEnumerator CanBeSetInterval()
        {
            yield return UniTask.ToCoroutine(async () =>
            {
                const long interval = 100L;
                IPausableTimer timer = new PausableTimer(true);
                timer.SetInterval(interval);
                var elapsed = 0L;
                timer.AddOnElapsed(args => elapsed = args.Elapsed);
                await timer.StartAsync();
                Assert.That(elapsed, Is.GreaterThanOrEqualTo(interval), $"{interval} elapsed.");
            });
        }

        [UnityTest]
        public IEnumerator CanBeAddAndRemoveListener()
        {
            var listener = new PausableTimerListener();
            Assert.That(listener.CallOnStarted, Is.False, "Timer is not started.");
            Assert.That(listener.CallOnStopped, Is.False, "Timer is not stopped.");
            Assert.That(listener.CallOnPaused, Is.False, "Timer is not paused.");
            Assert.That(listener.CallOnResumed, Is.False, "Timer is not resumed.");
            Assert.That(listener.CallOnElapsed, Is.False, "Timer is not elapsed.");

            IPausableTimer timer = new PausableTimer(true, 100);
            timer.AddListener(listener);
            timer.Start();

            Assert.That(listener.CallOnStarted, Is.True, "Timer is started.");
            Assert.That(listener.CallOnStopped, Is.False, "Timer is not stopped.");
            Assert.That(listener.CallOnPaused, Is.False, "Timer is not paused.");
            Assert.That(listener.CallOnResumed, Is.False, "Timer is not resumed.");
            Assert.That(listener.CallOnElapsed, Is.False, "Timer is not elapsed.");

            yield return UniTask.ToCoroutine(async () =>
            {
                await UniTask.Delay(50);
                timer.Pause();

                Assert.That(listener.CallOnStarted, Is.True, "Timer is started.");
                Assert.That(listener.CallOnStopped, Is.False, "Timer is not stopped.");
                Assert.That(listener.CallOnPaused, Is.True, "Timer is paused.");
                Assert.That(listener.CallOnResumed, Is.False, "Timer is not resumed.");
                Assert.That(listener.CallOnElapsed, Is.False, "Timer is not elapsed.");

                timer.Resume();

                Assert.That(listener.CallOnStarted, Is.True, "Timer is started.");
                Assert.That(listener.CallOnStopped, Is.False, "Timer is not stopped.");
                Assert.That(listener.CallOnPaused, Is.True, "Timer is paused.");
                Assert.That(listener.CallOnResumed, Is.True, "Timer is resumed.");
                Assert.That(listener.CallOnElapsed, Is.False, "Timer is not elapsed.");

                await UniTask.Delay(100);

                Assert.That(listener.CallOnStarted, Is.True, "Timer is started.");
                Assert.That(listener.CallOnStopped, Is.True, "Timer is stopped.");
                Assert.That(listener.CallOnPaused, Is.True, "Timer is paused.");
                Assert.That(listener.CallOnResumed, Is.True, "Timer is resumed.");
                Assert.That(listener.CallOnElapsed, Is.True, "Timer is elapsed.");

                listener.Reset();

                Assert.That(listener.CallOnStarted, Is.False, "Timer is not started.");
                Assert.That(listener.CallOnStopped, Is.False, "Timer is not stopped.");
                Assert.That(listener.CallOnPaused, Is.False, "Timer is not paused.");
                Assert.That(listener.CallOnResumed, Is.False, "Timer is not resumed.");
                Assert.That(listener.CallOnElapsed, Is.False, "Timer is not elapsed.");

                timer.RemoveListener(listener);
                await timer.StartAsync();

                Assert.That(listener.CallOnStarted, Is.False, "Timer is not started.");
                Assert.That(listener.CallOnStopped, Is.False, "Timer is not stopped.");
                Assert.That(listener.CallOnPaused, Is.False, "Timer is not paused.");
                Assert.That(listener.CallOnResumed, Is.False, "Timer is not resumed.");
                Assert.That(listener.CallOnElapsed, Is.False, "Timer is not elapsed.");
            });
        }
    }
}
