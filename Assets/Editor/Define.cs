using UnityEditor;
using UnityEngine;

namespace UnityXFrame.Editor
{
    public class Define
    {
        [MenuItem("Tools/Build Assets")]
        public static void BuildAB()
        {
            EditorWindow.GetWindow<BuildEditor>().Show();
        }

        [MenuItem("Tools/To Persist Folder")]
        public static void ToPersistFolder()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }

        [MenuItem("Tools/Test GUI Skin")]
        public static void TestGUISkin()
        {
            EditorWindow.GetWindow<TestGUISkinWindow>().Show();
        }
    }
}