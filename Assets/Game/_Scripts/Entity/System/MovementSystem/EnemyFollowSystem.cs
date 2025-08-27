using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class EnemyFollowSystem : IEntitySystem
    {
        private Transform _targetTransform;
        private NavMeshAgent _agent;

        public EnemyFollowSystemConfig Config { get; private set; }

        public EnemyFollowSystem(EnemyFollowSystemConfig config)
        {
            Config = config;
        }

        public void Init(Entity entity)
        {
            if (entity is UnityEntity unityEntity)
            {
                _agent = unityEntity.ViewTransform.GetComponent<NavMeshAgent>();
                if (_agent == null)
                {
                    _agent = unityEntity.ViewTransform.gameObject.AddComponent<NavMeshAgent>();
                }

                _agent.stoppingDistance = Config.StoppingDistance;
                _agent.speed = Config.Speed;
            }

            var player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                _targetTransform = player.transform;
            }
        }

        public void Update()
        {
            if (_targetTransform != null)
            {
                _agent.SetDestination(_targetTransform.position);
            }
        }

        public void OnPause()
        {
            _agent.SetDestination(_agent.gameObject.transform.position);
        }
    }
}
