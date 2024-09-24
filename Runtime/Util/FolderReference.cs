using UnityEditor;

namespace CupkekGames.Core
{
    [System.Serializable]
    public class FolderReference
    {
        public string GUID;
        public string Path => AssetDatabase.GUIDToAssetPath(GUID);
    }
}