using System;
using XFrame.Modules.Pools;

namespace UnityXFrame.Core.Audios
{
    public interface IAudio : IPoolObject
    {
        IAudioGroup Group { get; }
        void Play(Action callback = null);
        void PlayLoop();
        void Stop();
    }
}
