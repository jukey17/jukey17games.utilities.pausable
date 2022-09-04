using System.Collections;
using Jukey17Games.Utilities.Pausable;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

namespace Jukey17games.Utilities.Pausable.Tests.Runtime
{
    internal sealed class AudioSourcePauseSwitcherTest
    {
        [UnityTest]
        public IEnumerator CanBePauseAndResume()
        {
            var go = Utils.CreatePrimitive(PrimitiveType.Cube);
            var audioSource = go.AddComponent<AudioSource>();
            var _ = go.AddComponent<AudioListener>();
            IAudioSourcePauseSwitcher switcher = go.AddComponent<AudioSourcePauseSwitcher>();

            var sampleRate = 44100;
            audioSource.clip = AudioClip.Create("test", sampleRate * 5, 1, sampleRate, true);
            audioSource.Play();
            switcher.Pause();
            yield return new WaitForSeconds(1.0f);
            Assert.That(audioSource.isPlaying, Is.False, "audio source is not playing.");
            Assert.That(switcher.IsPausing, Is.True, "switcher is paused.");
            switcher.Resume();
            Assert.That(audioSource.isPlaying, Is.True, "audio source is playing.");
            Assert.That(switcher.IsPausing, Is.False, "switcher is resumed.");
        }
    }
}
