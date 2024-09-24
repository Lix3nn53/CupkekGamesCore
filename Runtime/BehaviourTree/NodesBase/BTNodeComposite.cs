using System.Collections.Generic;
using UnityEngine;

namespace CupkekGames.Core
{
    public abstract class BTNodeComposite : BTNode
    {
        public List<BTNode> Children = new();
        public override BTNode Clone()
        {
            BTNodeComposite clone = Instantiate(this);

            clone.Children = new();

            foreach (BTNode child in Children)
            {
                if (child == null)
                {
                    continue;
                }

                clone.Children.Add(child.Clone());
            }

            return clone;
        }
    }
}