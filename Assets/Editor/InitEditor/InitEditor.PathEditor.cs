using System.IO;
using UnityEditor;
using UnityEngine;
using UnityXFrame.Core;

namespace UnityXFrame.Editor
{
    public partial class InitEditor
    {
        private class PathEditor : DataEditorBase
        {
            protected override void OnInit()
            {
                m_Data.ArchivePath = Constant.ArchivePath;
                EditorUtility.SetDirty(m_Data);
            }

            public override void OnUpdate()
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
        }
    }
}
