using System.IO;
using UnityEditor;
using UnityEngine;
using UnityXFrame.Core;

namespace UnityXFrame.Editor
{
    public partial class InitEditor
    {
        private class PathEditor : IDataEditor
        {
            private InitData m_Data;

            public void OnInit(InitData data)
            {
                m_Data = data;
                m_Data.ArchivePath = Constant.ArchivePath;
                EditorUtility.SetDirty(m_Data);
            }

            public void OnUpdate()
            {
                EditorGUILayout.BeginHorizontal();
                Utility.Lable("ArchivePath");
                if (GUILayout.Button(m_Data.ArchivePath))
                {
                    if (!Directory.Exists(m_Data.ArchivePath))
                        Directory.CreateDirectory(m_Data.ArchivePath);
                    EditorUtility.RevealInFinder(m_Data.ArchivePath);
                }
                EditorGUILayout.EndHorizontal();
            }

            public void OnDestroy()
            {

            }
        }
    }
}
