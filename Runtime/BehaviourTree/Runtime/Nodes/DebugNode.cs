using UnityEngine;

namespace CupkekGames.Core
{
    public class DebugNode : BTNodeAction
    {
        public string Message;

        public override BTNodeRuntimeState OnUpdate()
        {
            Debug.Log($"{Message}");

            return BTNodeRuntimeState.Success;
        }
    }
}