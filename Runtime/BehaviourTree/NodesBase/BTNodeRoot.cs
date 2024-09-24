using UnityEngine;

namespace CupkekGames.Core
{
    public class BTNodeRoot : BTNodeDecorator
    {
        public override BTNodeRuntimeState OnUpdate()
        {
            return Child.Update();
        }
    }
}