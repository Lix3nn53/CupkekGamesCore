namespace CupkekGames.Core
{
    public abstract class BTNodeAction : BTNode
    {
        public override BTNode Clone()
        {
            return Instantiate(this);
        }
    }
}