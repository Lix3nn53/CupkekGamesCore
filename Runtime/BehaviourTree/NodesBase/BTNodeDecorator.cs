using UnityEngine;

namespace CupkekGames.Core
{
    public abstract class BTNodeDecorator : BTNode
    {
        public BTNode Child;
        public override BTNode Clone()
        {
            BTNodeDecorator clone = Instantiate(this);

            clone.Child = Child.Clone();

            return clone;
        }
    }
}