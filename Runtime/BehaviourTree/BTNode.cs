using System;
using UnityEngine;

namespace CupkekGames.Core
{
    public abstract class BTNode : ScriptableObject
    {
        [HideInInspector] public SerializedGuid Guid = new SerializedGuid(System.Guid.NewGuid());
        [HideInInspector] public Vector2 Position;
        [NonSerialized] public BTNodeRuntimeState State = BTNodeRuntimeState.Idle;
        public BTNodeRuntimeState Update()
        {
            State = OnUpdate();

            return State;
        }
        public virtual BTNodeRuntimeState OnUpdate()
        {
            return BTNodeRuntimeState.Success;
        }
        public abstract BTNode Clone();
    }
}