#if UNITY_ADDRESSABLES && UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace CupkekGames.Core.CGEditor
{
    public abstract class PrefabLoaderAddressableEditor<TKey, TValue> : Editor
    {
        bool _buttonEnabled = true;
        public override void OnInspectorGUI()
        {
            // Draw the default inspector
            DrawDefaultInspector();

            // Reference to the target script
            PrefabLoaderAddressable<TKey> prefabLoader = (PrefabLoaderAddressable<TKey>)target;

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
                $"Search for Addressable Prefabs with Label containing {typeof(TValue).Name} and add them to the list.\n" +
                "This usually takes less than 5 seconds, but may vary with project size." +
                "You may need to click elsewhere and then click this object again to refresh it.",
                labelStyle
            );

            GUI.enabled = _buttonEnabled;
            string buttonName = _buttonEnabled ? "Find All Addressable Prefabs With Label" : "Searching...";

            if (GUILayout.Button(buttonName))
            {
                _buttonEnabled = false;
                // Call the method when the button is pressed
                string label = serializedObject.FindProperty("_searchLabel").stringValue;

                Debug.Log($"Searching Addressable Prefabs with label {label}...");

                FindAddressablePrefabs(prefabLoader, label);
            }

            GUI.enabled = true;

            if (GUILayout.Button("Clear"))
            {
                prefabLoader.EditorClear();

                EditorUtility.SetDirty(prefabLoader);
                Repaint();
            }
        }

        private void FindAddressablePrefabs(PrefabLoaderAddressable<TKey> prefabLoader, string label)
        {
            Dictionary<TKey, string> prefabsWithScript = new Dictionary<TKey, string>();

            // Search for all addressable assets with the label "Prefab"
            Addressables.LoadResourceLocationsAsync(label, typeof(GameObject)).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    int handleCount = 0;

                    foreach (IResourceLocation location in handle.Result)
                    {
                        handleCount++;

                        Addressables.LoadAssetAsync<GameObject>(location).Completed += prefabHandle =>
                        {
                            if (prefabHandle.Status == AsyncOperationStatus.Succeeded)
                            {
                                GameObject prefab = prefabHandle.Result;
                                if (prefab != null && prefab.GetComponent<TValue>() != null)
                                {
                                    string guid;
                                    long file;
                                    if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(prefab, out guid, out file))
                                    {
                                        prefabsWithScript.Add(GetKeyFromFileName(prefab.name), guid);
                                    }
                                }
                            }

                            handleCount--;
                            if (handleCount == 0)
                            {
                                AfterLoad(prefabLoader, prefabsWithScript);
                            }
                        };
                    }
                }
                else
                {
                    Debug.LogError("Failed to load addressable locations.");
                }
            };
        }

        private void AfterLoad(PrefabLoaderAddressable<TKey> prefabLoader, Dictionary<TKey, string> prefabsWithScript)
        {
            if (prefabsWithScript.Count > 0)
            {
                Debug.Log($"Found {prefabsWithScript.Count} addressable prefabs with the script {typeof(TValue).Name}:");
                foreach (var kvp in prefabsWithScript)
                {
                    prefabLoader.EditorAdd(kvp.Key, new AssetReference(kvp.Value));
                }

                EditorUtility.SetDirty(prefabLoader);
                Repaint();
            }
            else
            {
                Debug.Log($"No addressable prefabs found with the script {typeof(TValue).Name}.");
            }

            _buttonEnabled = true;
        }

        public abstract TKey GetKeyFromFileName(string name);

    }
}
#endif