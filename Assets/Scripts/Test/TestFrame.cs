using UnityXFrame.Core.UIs;
using UnityXFrame.Core.Diagnotics;
using UnityXFrame.Core.Audios;
using XFrame.Modules.Tasks;
using XFrame.Modules.Resource;
using System;
using UnityXFrame.Core;
using UnityEngine;

namespace Game.Test
{
    [DebugHelp("框架测试窗口")]
    public class TestFrame : IDebugWindow
    {
        private int m_Group;
        private int m_UI = 1;
        private int m_Layer;
        private int m_GroupLayer;

        private IAudio m_Audio1;
        private IAudio m_Audio2;

        public void Dispose()
        {

        }

        public void OnAwake()
        {

        }

        public void OnDraw()
        {
            m_UI = DebugGUI.IntField(m_UI);
            m_Group = DebugGUI.IntField(m_Group);
            m_Layer = DebugGUI.IntField(m_Layer);
            m_GroupLayer = DebugGUI.IntField(m_GroupLayer);
            if (DebugGUI.Button("Open UI"))
                UIModule.Inst.Open($"TestUI{m_UI}", default, true);
            if (DebugGUI.Button("Close UI"))
                UIModule.Inst.Close($"TestUI{m_UI}");
            if (DebugGUI.Button("Open UI To Group"))
                UIModule.Inst.Open($"TestUI{m_UI}", $"Group{m_UI}", default, true);
            if (DebugGUI.Button("Set Layer"))
                UIModule.Inst.Get($"TestUI{m_UI}").Layer = m_Layer;
            if (DebugGUI.Button("Set Group Layer"))
                UIModule.Inst.MainGroup.Layer = m_GroupLayer;

            if (DebugGUI.Button("Test"))
            {
                Debug.LogWarning(GUI.skin.customStyles.Length);
            }
            if (DebugGUI.Button("Play bgm1"))
            {
                XTask<IAudio> task = AudioModule.Inst.PlayLoopAsync("TestAudio.mp3");
                task.OnComplete(() => m_Audio1 = task.Data);
            }
            if (DebugGUI.Button("Play1 bgm2"))
            {
                XTask<IAudio> task = AudioModule.Inst.PlayLoopAsync("TestAudio2.mp3");
                task.OnComplete(() => m_Audio2 = task.Data);
            }
            if (DebugGUI.Button("Stop bgm1"))
            {
                m_Audio1.Stop();
            }
            if (DebugGUI.Button("Stop bgm2"))
            {
                m_Audio2.Stop();
            }

            if (DebugGUI.Button("Test Preload"))
            {
                ResModule.Inst.Preload(
                    new string[] { $"{Constant.AUDIO_PATH}/TestAudio.mp3" },
                    new Type[] { typeof(AudioClip) })
                    .Start();
            }
            if (DebugGUI.Button("Play BGM1"))
                AudioModule.Inst.PlayLoop("TestAudio.mp3");

            if (DebugGUI.Button("Play1"))
                AudioModule.Inst.PlayAsync("a1.wav");
            if (DebugGUI.Button("Play2"))
                AudioModule.Inst.PlayAsync("a2.wav");
            if (DebugGUI.Button("Play3"))
                AudioModule.Inst.PlayAsync("a3.wav");
        }
    }
}
