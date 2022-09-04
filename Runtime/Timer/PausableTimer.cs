using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Jukey17Games.Utilities.Pausable
{
    public interface IPausableTimer : IPauseSwitcher, IPausable, IDisposable
    {
        bool IsStarting { get; }
        UniTask StartAsync(bool unpause = true, CancellationToken token = default);
        void Start(bool unpause = true);
        void Stop();
        void SetInterval(long interval, bool immediate = true);
        IPausableTimer AddListener(IPausableTimerListener listener);
        IPausableTimer RemoveListener(IPausableTimerListener listener);
        IPausableTimer ClearListener();
    }

    public sealed class PausableTimer : IPausableTimer
    {
        public bool IsStarting { get; private set; }
        public bool IsPausing { get; private set; }

        private readonly bool _oneShot;
        private readonly PlayerLoopTiming _playerLoopTiming;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly IList<IPausableTimerListener> _listeners = new List<IPausableTimerListener>();

        private long _interval;
        private long _prevDivided;
        private Action _applyIntervalFunc;
        private bool _disposed;

        public PausableTimer(bool oneShot)
        {
            _oneShot = oneShot;
            _playerLoopTiming = PlayerLoopTiming.Update;
        }

        public PausableTimer(bool oneShot, long interval)
        {
            _oneShot = oneShot;
            _interval = interval;
            _playerLoopTiming = PlayerLoopTiming.Update;
        }

        public PausableTimer(bool oneShot, long interval, PlayerLoopTiming playerLoopTiming)
        {
            _oneShot = oneShot;
            _interval = interval;
            _playerLoopTiming = playerLoopTiming;
        }

        public async UniTask StartAsync(bool unpause = true, CancellationToken token = default)
        {
            if (_disposed)
            {
                return;
            }

            if (unpause)
            {
                IsPausing = false;
            }

            _prevDivided = 0;
            IsStarting = true;
            _stopwatch.Restart();
            var eventArgs = new PausableTimerEventArgs(_interval, 0, 0, _oneShot);
            DispatchOnStarted(eventArgs);

            await WaitElapsedAsync(token);
        }

        public void Start(bool unpause = true)
        {
            if (_disposed)
            {
                return;
            }

            StartAsync(unpause).Forget();
        }

        public void Stop()
        {
            if (_disposed)
            {
                return;
            }

            IsStarting = false;
            var eventArgs = new PausableTimerEventArgs(_interval, _stopwatch.ElapsedMilliseconds, (int) _prevDivided,
                _oneShot);
            DispatchOnStopped(eventArgs);

            _stopwatch.Stop();
            _stopwatch.Reset();
        }

        public void Pause()
        {
            ((IPausable) this).OnPaused();
        }

        public void Resume()
        {
            ((IPausable) this).OnPaused();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            Stop();
            ClearListener();
            _disposed = true;
        }

        public void SetInterval(long interval, bool immediate = true)
        {
            if (_disposed)
            {
                return;
            }

            void Apply()
            {
                _stopwatch.Restart();
                _interval = interval;
                _prevDivided = 0;
            }

            if (immediate)
            {
                Apply();
            }
            else
            {
                _applyIntervalFunc = Apply;
            }
        }

        public IPausableTimer AddListener(IPausableTimerListener listener)
        {
            _listeners.Add(listener);
            return this;
        }

        public IPausableTimer RemoveListener(IPausableTimerListener listener)
        {
            _listeners.Remove(listener);
            return this;
        }

        public IPausableTimer ClearListener()
        {
            _listeners.Clear();
            return this;
        }

        void IPausable.OnPaused()
        {
            if (_disposed)
            {
                return;
            }

            if (IsPausing)
            {
                return;
            }

            IsPausing = true;
            _stopwatch.Stop();
            var eventArgs = new PausableTimerEventArgs(_interval, _stopwatch.ElapsedMilliseconds, (int) _prevDivided,
                _oneShot);
            DispatchOnPaused(eventArgs);
        }

        void IPausable.OnResumed()
        {
            if (_disposed)
            {
                return;
            }

            if (!IsPausing)
            {
                return;
            }

            IsPausing = false;
            _stopwatch.Start();
            var eventArgs = new PausableTimerEventArgs(_interval, _stopwatch.ElapsedMilliseconds, (int) _prevDivided,
                _oneShot);
            DispatchOnResumed(eventArgs);
        }

        private async UniTask WaitElapsedAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            while (IsStarting)
            {
                if (!IsPausing)
                {
                    var divided = _interval > 0 ? _stopwatch.ElapsedMilliseconds / _interval : _prevDivided + 1;
                    if (divided > _prevDivided)
                    {
                        var eventArgs = new PausableTimerEventArgs(_interval, _stopwatch.ElapsedMilliseconds,
                            (int) divided, _oneShot);
                        DispatchOnElapsed(eventArgs);

                        if (_oneShot)
                        {
                            _prevDivided = divided;
                            Stop();
                            break;
                        }

                        if (_applyIntervalFunc != null)
                        {
                            _applyIntervalFunc.Invoke();
                            _applyIntervalFunc = null;
                            divided = 0;
                        }
                    }

                    _prevDivided = divided;
                }

                await UniTask.Yield(_playerLoopTiming, token);
            }
        }

        private void DispatchOnStarted(in PausableTimerEventArgs eventArgs)
        {
            foreach (var listener in _listeners)
            {
                listener.OnStarted(eventArgs);
            }
        }

        private void DispatchOnStopped(in PausableTimerEventArgs eventArgs)
        {
            foreach (var listener in _listeners)
            {
                listener.OnStopped(eventArgs);
            }
        }

        private void DispatchOnPaused(in PausableTimerEventArgs eventArgs)
        {
            foreach (var listener in _listeners)
            {
                listener.OnPaused(eventArgs);
            }
        }

        private void DispatchOnResumed(in PausableTimerEventArgs eventArgs)
        {
            foreach (var listener in _listeners)
            {
                listener.OnResumed(eventArgs);
            }
        }

        private void DispatchOnElapsed(in PausableTimerEventArgs eventArgs)
        {
            foreach (var listener in _listeners)
            {
                listener.OnElapsed(eventArgs);
            }
        }
    }
}
