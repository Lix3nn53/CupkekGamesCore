#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CupkekGames.Core.CGEditor
{
    public abstract class PrefabLoaderClassicEditor<TKey, TValue> : Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector
            DrawDefaultInspector();

            // Reference to the target script
            PrefabLoaderClassic<TKey> prefabLoader = (PrefabLoaderClassic<TKey>)target;

            // if (prefabLoader.EditorHasDuplicateKeys())
            // {
            //     EditorGUILayout.HelpBox("Duplicate keys, please ensure all keys are unique.", MessageType.Warning);
            // }

            // Add a button to the inspector
            GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
            {
                wordWrap = true
            };

            GUILayout.Label(
                $"Search for Prefabs containing {typeof(TValue).Name} in folder and add them to the list.\n" +
                "This usually takes less than 5 seconds, but may vary with project size." +
                "You may need to click elsewhere and then click this object again to refresh it.",
                labelStyle
            );

            if (GUILayout.Button("Find All Prefabs In Folder"))
            {
                // Call the method when the button is pressed
                SerializedProperty folder = serializedObject.FindProperty("_searchFolder");
                string guid = folder.FindPropertyRelative("GUID").stringValue;
                string searchFolder = AssetDatabase.GUIDToAssetPath(guid);

                Debug.Log($"Searching folder {searchFolder}...");

                FindPrefabs(prefabLoader, searchFolder);
            }

            if (GUILayout.Button("Clear"))
            {
                prefabLoader.EditorClear();

                EditorUtility.SetDirty(prefabLoader);
                Repaint();
            }
        }

        private void FindPrefabs(PrefabLoaderClassic<TKey> prefabLoader, string searchFolder)
        {
            List<GameObject> prefabsWithScript = new List<GameObject>();

            // Get all prefab paths in the project
            string[] prefabPaths = AssetDatabase.FindAssets("t:Prefab", new[] { searchFolder });

            foreach (string prefabPath in prefabPaths)
            {
                string fullPath = AssetDatabase.GUIDToAssetPath(prefabPath);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(fullPath);

                if (prefab.GetComponent<TValue>() != null)
                {
                    prefabsWithScript.Add(prefab);
                }
            }

            if (prefabsWithScript.Count > 0)
            {
                Debug.Log($"Found {prefabsWithScript.Count} prefabs with the script {typeof(TValue).Name}:");
                foreach (GameObject prefab in prefabsWithScript)
                {
                    prefabLoader.EditorAdd(GetKeyFromFileName(prefab.name), prefab);
                }

                EditorUtility.SetDirty(prefabLoader);
                Repaint();
            }
            else
            {
                Debug.Log($"No prefabs found with the script {typeof(TValue).Name}.");
            }
        }
        public abstract TKey GetKeyFromFileName(string name);
    }
}
#endif