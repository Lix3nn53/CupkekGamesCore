
using System;

[Serializable]
public struct KeyValuePair<TKey, TValue>
{
    public TKey Key;
    public TValue Value;
}