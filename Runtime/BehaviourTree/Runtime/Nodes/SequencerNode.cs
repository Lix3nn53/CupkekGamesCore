
using UnityEngine;

namespace CupkekGames.Core
{
    public class SequencerNode : BTNodeComposite
    {
        public bool ExitOnFail = false;
        private int _index = 0;
        private BTNodeRuntimeState _lastState;
        public override BTNodeRuntimeState OnUpdate()
        {
            if (_index == Children.Count)
            {
                // Reset and loop if update called again after squence is done
                _index = 0;
            }

            BTNode child = Children[_index];

            if (child == null)
            {
                _lastState = BTNodeRuntimeState.Success;
            }
            else
            {
                _lastState = child.Update();
            }

            if (ExitOnFail && _lastState == BTNodeRuntimeState.Fail)
            {
                return BTNodeRuntimeState.Fail;
            }

            if (_lastState == BTNodeRuntimeState.Success || _lastState == BTNodeRuntimeState.Fail)
            {
                _index++;

                if (_index == Children.Count)
                {
                    return BTNodeRuntimeState.Success;
                }
            }

            return BTNodeRuntimeState.Running;
        }
    }
}