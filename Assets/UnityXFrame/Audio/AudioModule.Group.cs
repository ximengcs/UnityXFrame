
using XFrame.Modules.Pools;

namespace UnityXFrame.Core.Audios
{
    public partial class AudioModule
    {
        private class Group : IAudioGroup
        {
            public void Add(IAudio audio)
            {

            }

            public void Remove(IAudio audio)
            {

            }

            public void Play()
            {

            }

            public void Stop()
            {

            }

            void IPoolObject.OnCreate()
            {
                throw new System.NotImplementedException();
            }

            void IPoolObject.OnRelease()
            {
                throw new System.NotImplementedException();
            }

            void IPoolObject.OnDelete()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
