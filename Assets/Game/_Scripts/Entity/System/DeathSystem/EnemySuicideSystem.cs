using System;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class EnemySuicideSystem : IEntitySystem
    {
        private Transform _targetTransform;
        private NavMeshAgent _agent;
        private HealthSystem _playerHealth;
        private UnityEntity _unityEntity;

        public EnemySuicideSystemConfig Config { get; private set; }

        public EnemySuicideSystem(EnemySuicideSystemConfig config)
        {
            Config = config;
        }

        public event Action<UnityEntity> OnDestroyed;

        public void Init(Entity entity)
        {
            if (entity is UnityEntity unityEntity)
            {
                _unityEntity = unityEntity;
                _agent = unityEntity.ViewTransform.GetComponent<NavMeshAgent>();
            }

            var player = GameObject.FindWithTag("Player");
            var entityView = player.GetComponent<EntityView>();
            _targetTransform = player.transform;
            _playerHealth = entityView.Entity.GetSystem<HealthSystem>();
        }

        public void Update()
        {
            if (_targetTransform == null || _agent == null)
                return;

            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
                Explode();
        }

        private void Explode()
        {
            _playerHealth.TakeDamage(Config.Damage);
            _unityEntity.ViewTransform.gameObject.SetActive(false);
            OnDestroyed?.Invoke(_unityEntity);
        }
    }
}
