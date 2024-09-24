#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace CupkekGames.Core.CGEditor
{
    [CustomEditor(typeof(PrefabLoaderClassicString), true)]
    public class PrefabLoaderClassicStringEditor : PrefabLoaderClassicEditor<string, GameObject>
    {
        public override string GetKeyFromFileName(string name)
        {
            return name;
        }
    }
}
#endif