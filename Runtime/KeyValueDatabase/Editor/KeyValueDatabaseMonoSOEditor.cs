#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CupkekGames.Core.CGEditor
{
    public abstract class KeyValueDatabaseMonoSOEditor<TKey, TValue> : Editor where TValue : ScriptableObject
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector
            DrawDefaultInspector();

            // Reference to the target script
            KeyValueDatabaseMonoSO<TKey, TValue> scriptableObjectDatabase = (KeyValueDatabaseMonoSO<TKey, TValue>)target;

            // if (scriptableObjectDatabase.EditorHasDuplicateKeys())
            // {
            //     EditorGUILayout.HelpBox("Duplicate keys, please ensure all keys are unique.", MessageType.Warning);
            // }

            // Add a button to the inspector
            GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
            {
                wordWrap = true
            };

            GUILayout.Label(
                $"Search for ScriptableObjects of type {typeof(TValue).Name} in folder and add them to the list.\n" +
                "This usually takes less than 5 seconds, but may vary with project size." +
                "You may need to click elsewhere and then click this object again to refresh it.",
                labelStyle
            );

            if (GUILayout.Button("Find All ScriptableObjects In Folder"))
            {
                // Call the method when the button is pressed
                SerializedProperty folder = serializedObject.FindProperty("_searchFolder");
                string guid = folder.FindPropertyRelative("GUID").stringValue;
                string searchFolder = AssetDatabase.GUIDToAssetPath(guid);

                Debug.Log($"Searching folder {searchFolder}...");

                FindScriptableObjects(scriptableObjectDatabase, searchFolder);
            }

            if (GUILayout.Button("Clear"))
            {
                scriptableObjectDatabase.EditorClear();

                EditorUtility.SetDirty(scriptableObjectDatabase);
                Repaint();
            }
        }

        private void FindScriptableObjects(KeyValueDatabaseMonoSO<TKey, TValue> scriptableObjectDatabase, string searchFolder)
        {
            List<TValue> scriptableObjects = new List<TValue>();

            // Get all ScriptableObject paths in the project
            string[] scriptableObjectPaths = AssetDatabase.FindAssets($"t:{typeof(TValue).Name}", new[] { searchFolder });

            foreach (string scriptableObjectPath in scriptableObjectPaths)
            {
                string fullPath = AssetDatabase.GUIDToAssetPath(scriptableObjectPath);
                TValue scriptableObject = AssetDatabase.LoadAssetAtPath<TValue>(fullPath);

                if (scriptableObject != null)
                {
                    scriptableObjects.Add(scriptableObject);
                }
            }

            if (scriptableObjects.Count > 0)
            {
                Debug.Log($"Found {scriptableObjects.Count} ScriptableObjects of type {typeof(TValue).Name}:");
                foreach (TValue scriptableObject in scriptableObjects)
                {
                    scriptableObjectDatabase.EditorAdd(GetKeyFromFileName(scriptableObject.name), scriptableObject);
                }

                EditorUtility.SetDirty(scriptableObjectDatabase);
                Repaint();
            }
            else
            {
                Debug.Log($"No ScriptableObjects found of type {typeof(TValue).Name}.");
            }
        }

        public abstract TKey GetKeyFromFileName(string name);
    }
}
#endif
