namespace Jukey17Games.Utilities.Pausable
{
    public interface IPauseSwitcher
    {
        bool IsPausing { get; }
        void Pause();
        void Resume();
    }
}
