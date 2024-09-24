
using UnityEngine;

namespace CupkekGames.Core
{
    public class RepeatNode : BTNodeDecorator
    {
        [Header("(RepeatAmount <= 0) for infinite")]
        public int RepeatAmount;
        public bool ExitOnFail = false;
        private int _repeated = 0;
        public override BTNodeRuntimeState OnUpdate()
        {
            BTNodeRuntimeState state = Child.Update();

            if (ExitOnFail && state == BTNodeRuntimeState.Fail)
            {
                return BTNodeRuntimeState.Fail;
            }

            if (state == BTNodeRuntimeState.Success || state == BTNodeRuntimeState.Fail)
            {
                if (_repeated + 1 == RepeatAmount)
                {
                    return BTNodeRuntimeState.Success;
                }

                _repeated++;
            }

            return BTNodeRuntimeState.Running;
        }
    }
}