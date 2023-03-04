using UnityEditor;
using UnityEngine.Audio;
using UnityXFrame.Core;

namespace UnityXFrame.Editor
{
    public partial class InitEditor
    {
        private class AudioEditor : IDataEditor
        {
            private InitData m_Data;
            private AudioMixer m_File;

            public void OnInit(InitData data)
            {
                m_Data = data;
            }

            public void OnUpdate()
            {
                EditorGUILayout.BeginHorizontal();
                Utility.Lable("AudioMixer");
                m_File = (AudioMixer)EditorGUILayout.ObjectField(m_File, typeof(AudioMixer), false);
                EditorGUILayout.EndHorizontal();

                if (m_File != m_Data.AudioMixer)
                {
                    m_Data.AudioMixer = m_File;
                    EditorUtility.SetDirty(m_Data);
                }
            }

            public void OnDestroy()
            {

            }
        }
    }
}
