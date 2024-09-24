using System;
using UnityEngine;

namespace CupkekGames.Core
{
    [Serializable]
    public struct SerializedGuid
    {
        [SerializeField] private string _value;
        public readonly string ValueStr => _value;
        public readonly Guid Value
        {
            get
            {
                if (string.IsNullOrEmpty(_value))
                {
                    return Guid.Empty;
                }

                return new Guid(_value);
            }
        }
        public SerializedGuid(string value = "")
        {
            if (Guid.TryParse(value, out Guid result))
            {
                _value = value;
            }
            else
            {
                _value = Guid.Empty.ToString();
            }
        }
        public SerializedGuid(Guid value)
        {
            _value = value.ToString();
        }

        public static SerializedGuid NewGUID()
        {
            return new SerializedGuid(Guid.NewGuid().ToString());
        }

        public static implicit operator string(SerializedGuid guid) => guid.ValueStr;
    }
}