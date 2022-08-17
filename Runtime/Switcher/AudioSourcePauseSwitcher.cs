using UnityEngine;

namespace Jukey17Games.Utilities.Pausable
{
    public interface IAudioSourcePauseSwitcher : IPauseSwitcher, IPausable
    {
    }

    [RequireComponent(typeof(AudioSource))]
    public sealed class AudioSourcePauseSwitcher : MonoBehaviour, IAudioSourcePauseSwitcher
    {
        private AudioSource _audioSource;

        public bool IsPausing { get; private set; }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void Pause()
        {
            ((IPausable) this).OnPaused();
        }

        public void Resume()
        {
            ((IPausable) this).OnResumed();
        }

        void IPausable.OnPaused()
        {
            if (IsPausing)
            {
                return;
            }

            IsPausing = true;
            _audioSource.Pause();
        }

        void IPausable.OnResumed()
        {
            if (!IsPausing)
            {
                return;
            }

            IsPausing = false;
            _audioSource.UnPause();
        }
    }
}
