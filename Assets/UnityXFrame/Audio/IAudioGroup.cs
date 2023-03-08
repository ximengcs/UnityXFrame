
namespace UnityXFrame.Core.Audios
{
    public interface IAudioGroup
    {
        void Play();
        void Stop();
        void Add(IAudio audio);
    }
}
