#if UNITY_EDITOR
using UnityEditor;

namespace CupkekGames.Core.CGEditor
{
    [CustomEditor(typeof(SceneDatabase))]
    public class SceneDatabaseEditor : KeyValueDatabaseMonoSOEditor<string, SceneSO>
    {
        public override string GetKeyFromFileName(string name)
        {
            return name;
        }
    }
}
#endif
