using UnityEngine;

namespace CupkekGames.Core
{
    public class DelayNode : BTNodeDecorator
    {
        public float Duration = 1;
        private float _startTime = -1f;

        public override BTNodeRuntimeState OnUpdate()
        {
            if (_startTime < 0f)
            {
                _startTime = Time.time;
            }

            if (Time.time - _startTime > Duration)
            {
                var state = Child.Update();

                if (state == BTNodeRuntimeState.Success || state == BTNodeRuntimeState.Fail)
                {
                    // Reset for next call
                    _startTime = -1f;
                }

                return state;
            }

            return BTNodeRuntimeState.Running;
        }
    }
}
