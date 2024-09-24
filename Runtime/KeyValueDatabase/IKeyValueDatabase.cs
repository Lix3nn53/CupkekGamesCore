namespace CupkekGames.Core
{
  public interface IKeyValueDatabase<TKey, TValue>
  {
    public bool EditorHasKey(TKey key);
    public bool EditorAdd(TKey key, TValue value);
    public bool EditorHasDuplicateKeys();
    public void EditorClear();
  }
}