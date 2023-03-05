using System;
using XFrame.Core;
using UnityEngine;
using UnityEngine.Audio;
using XFrame.Modules.Pools;
using XFrame.Modules.Tasks;
using XFrame.Modules.Resource;

namespace UnityXFrame.Core.Audios
{
    [CoreModule]
    public partial class AudioModule : SingletonModule<AudioModule>
    {
        private Transform m_Root;
        private AudioMixer m_Mixer;
        private AudioMixerGroup m_MainGroup;
        private IPool<Audio> m_AudioPool;
        private IPool<Group> m_GroupPool;

        protected override void OnInit(object data)
        {
            base.OnInit(data);
        }

        protected override void OnStart()
        {
            base.OnStart();

            m_Root = new GameObject("Audios").transform;
            m_AudioPool = PoolModule.Inst.GetOrNew<Audio>();
            m_GroupPool = PoolModule.Inst.GetOrNew<Group>();
            m_Mixer = Init.Inst.Data.AudioMixer;
            m_MainGroup = m_Mixer.FindMatchingGroups("Master")[0];
        }

        public IAudioGroup GetOrNewGroup(string groupName)
        {
            return default;
        }

        public IAudio Get(string name)
        {
            AudioClip clip = ResModule.Inst.Load<AudioClip>($"{Constant.AUDIO_PATH}/{name}");
            if (m_AudioPool.Require(out Audio audio))
                audio.OnInit(m_Root, m_MainGroup, clip);
            else
                audio.Clip = clip;
            audio.OnDispose(() => m_AudioPool.Release(audio));
            return audio;
        }

        public XTask<IAudio> GetAsync(string name)
        {
            XTask<IAudio> task = TaskModule.Inst.GetOrNew<XTask<IAudio>>();
            ResLoadTask<AudioClip> loadTask = ResModule.Inst.LoadAsync<AudioClip>($"{Constant.AUDIO_PATH}/{name}");
            loadTask.OnComplete((clip) =>
            {
                if (clip == null)
                    return;

                if (m_AudioPool.Require(out Audio audio))
                    audio.OnInit(m_Root, m_MainGroup, clip);
                else
                    audio.Clip = clip;
                task.Data = audio;
                audio.OnDispose(() => m_AudioPool.Release(audio));
            });
            task.Add(loadTask).Start();
            return task;
        }

        public IAudio Play(string name, Action callback = null)
        {
            IAudio audio = Get(name);
            audio.Play(callback);
            return audio;
        }

        public IAudio Play(string name, IAudioGroup group, Action callback = null)
        {
            return default;
        }

        public XTask<IAudio> PlayAsync(string name, Action callback = null)
        {
            XTask<IAudio> task = GetAsync(name);
            task.OnComplete(() => task.Data.Play(callback));
            return task;
        }

        public IAudio PlayLoop(string name)
        {
            IAudio audio = Get(name);
            audio.PlayLoop();
            return audio;
        }

        public XTask<IAudio> PlayLoopAsync(string name)
        {
            XTask<IAudio> task = GetAsync(name);
            task.OnComplete(() => task.Data.PlayLoop());
            return task;
        }
    }
}
