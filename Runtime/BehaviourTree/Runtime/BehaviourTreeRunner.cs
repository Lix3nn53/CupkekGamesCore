using UnityEngine;

namespace CupkekGames.Core
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        [SerializeField] BehaviourTree _behaviourTree;
        public BehaviourTree BehaviourTree => _behaviourTree;
        private BehaviourTree _runtimeClone;
        public BehaviourTree RuntimeClone => _runtimeClone;

        private void Awake()
        {
            _runtimeClone = _behaviourTree.Clone();
        }

        private void Update()
        {
            _runtimeClone.Update();
        }
    }
}