using UnityEngine;

namespace Jukey17Games.Utilities.Pausable
{
    public interface IAnimatorPauseSwitcher : IPauseSwitcher, IPausable
    {
    }

    [RequireComponent(typeof(Animator))]
    public sealed class AnimatorPauseSwitcher : MonoBehaviour, IAnimatorPauseSwitcher
    {
        private Animator _animator;
        private float _speed;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public bool IsPausing { get; private set; }

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
            _speed = _animator.speed;
            _animator.speed = 0.0f;
        }

        void IPausable.OnResumed()
        {
            if (!IsPausing)
            {
                return;
            }

            IsPausing = false;
            _animator.speed = _speed;
        }
    }
}
