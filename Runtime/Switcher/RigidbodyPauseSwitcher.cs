using UnityEngine;

namespace Jukey17Games.Utilities.Pausable
{
    public interface IRigidbodyPauseSwitcher : IPauseSwitcher, IPausable
    {
    }

    [RequireComponent(typeof(Rigidbody))]
    public sealed class RigidbodyPauseSwitcher : MonoBehaviour, IRigidbodyPauseSwitcher
    {
        private Rigidbody _rigidbody;
        private Vector3 _angularVelocity;
        private Vector3 _velocity;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
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
            _rigidbody.isKinematic = true;
            _angularVelocity = _rigidbody.angularVelocity;
            _velocity = _rigidbody.velocity;
        }

        void IPausable.OnResumed()
        {
            if (!IsPausing)
            {
                return;
            }

            IsPausing = false;
            _rigidbody.isKinematic = false;
            _rigidbody.angularVelocity = _angularVelocity;
            _rigidbody.velocity = _velocity;
        }
    }
}
