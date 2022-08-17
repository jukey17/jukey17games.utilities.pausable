using UnityEngine;

namespace Jukey17Games.Utilities.Pausable
{
    public interface IParticleSystemPauseSwitcher : IPauseSwitcher, IPausable
    {
    }

    [RequireComponent(typeof(ParticleSystem))]
    public sealed class ParticleSystemPauseSwitcher : MonoBehaviour, IParticleSystemPauseSwitcher
    {
        [SerializeField] private bool withChildren = true;

        private ParticleSystem _particleSystem;

        public bool IsPausing { get; private set; }

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
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

            if (_particleSystem.isPaused)
            {
                return;
            }

            IsPausing = true;
            _particleSystem.Pause(withChildren);
        }

        void IPausable.OnResumed()
        {
            if (!IsPausing)
            {
                return;
            }

            if (!_particleSystem.isPaused)
            {
                return;
            }

            IsPausing = false;
            _particleSystem.Play();
        }
    }
}
