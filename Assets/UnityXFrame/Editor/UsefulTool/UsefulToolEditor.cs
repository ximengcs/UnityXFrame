using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UnityXFrame.Editor
{
    public class UsefulToolEditor : EditorWindow
    {
        private void OnEnable()
        {
            titleContent = new GUIContent("Tool");
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Clear HotUpdate Folder"))
            {
                string path = HybridCLR.Editor.HybridCLRSettings.Instance.hotUpdateDllCompileOutputRootDir;
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
            }

            if (GUILayout.Button("Copy Hotfix to data"))
            {
                string path = HybridCLR.Editor.HybridCLRSettings.Instance.hotUpdateDllCompileOutputRootDir;
                path = Path.Combine(path, EditorUserBuildSettings.activeBuildTarget.ToString());
                path = Path.Combine(path, "Hotfix.dll");
                if (File.Exists(path))
                {
                    string targetPath = "Assets/Data/Dlls/Hotfix/Hotfix.Bytes";
                    File.Copy(path, targetPath, true);
                    AssetDatabase.Refresh();
                }
            }

            if (GUILayout.Button("Clear Remote Build Path"))
            {
                var setting = AddressableAssetSettingsDefaultObject.Settings;
                string path = setting.RemoteCatalogBuildPath.GetValue(setting);
                foreach (string file in Directory.EnumerateFiles(path))
                {
                    string ext = Path.GetExtension(file);
                    if (ext == ".hash" || ext == ".json")
                        continue;

                    File.Delete(file);
                }
            }
        }
    }
}
