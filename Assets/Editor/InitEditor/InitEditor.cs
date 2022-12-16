using System;
using XFrame.Core;
using UnityEditor;
using UnityEngine;
using UnityXFrame.Core;
using XFrame.Collections;
using XFrame.Modules.XType;
using UnityEditor.SceneManagement;

namespace UnityXFrame.Editor
{
    [CustomEditor(typeof(Init))]
    public partial class InitEditor : UnityEditor.Editor
    {
        public const string InitDataPath = "Assets/UnityXFrame/InitData.asset";

        private InitData m_Data;
        private TypeModule.System m_EditorType;
        private XLinkList<IDataEditor> m_Editors;

        private void OnEnable()
        {
            if (Application.isPlaying)
                return;
            Entry.Init();
            m_Editors = new XLinkList<IDataEditor>();
            m_EditorType = TypeModule.Inst.GetOrNew<IDataEditor>();
            m_Data = AssetDatabase.LoadAssetAtPath<InitData>(InitDataPath);
            if (m_Data == null)
            {
                m_Data = CreateInstance<InitData>();
                AssetDatabase.CreateAsset(m_Data, InitDataPath);
            }

            serializedObject.FindProperty("m_Data").objectReferenceValue = m_Data;
            serializedObject.ApplyModifiedProperties();
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

            foreach (Type type in m_EditorType)
            {
                IDataEditor editor = Activator.CreateInstance(type) as IDataEditor;
                editor.OnInit(m_Data);
                m_Editors.AddLast(editor);
            }
        }

        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
                return;
            XLinkNode<IDataEditor> node = m_Editors.First;
            while (node != null)
            {
                EditorGUILayout.BeginVertical(GUI.skin.customStyles[195]);
                node.Value.OnUpdate();
                EditorGUILayout.EndVertical();
                node = node.Next;
            }
        }

        private void OnDisable()
        {
            if (Application.isPlaying)
                return;

            XLinkNode<IDataEditor> node = m_Editors.First;
            while (node != null)
            {
                node.Value.OnDestroy();
                node = node.Next;
            }

            EditorUtility.SetDirty(m_Data);
            Entry.ShutDown();
            m_EditorType = null;
            m_Editors = null;
            m_Data = null;
        }
    }
}
