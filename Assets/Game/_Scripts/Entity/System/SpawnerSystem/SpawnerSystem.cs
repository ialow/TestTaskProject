using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SpawnerSystem : IEntitySystem
    {
        private UnityEntity _unityEntity;

        private Untils.PoolObject<EntityView> _poolEntities;
        private List<EntityView> _inActivation = new List<EntityView>();

        private float _timer;

        public SpawnerSystemConfig Config { get; private set; }

        public SpawnerSystem(SpawnerSystemConfig config)
        {
            Config = config;
        }

        public IReadOnlyList<EntityView> Enemies => _poolEntities.ActiveObjects;

        public void Init(Entity entity)
        {
            if (entity is UnityEntity unityEntity)
            {
                _unityEntity = unityEntity;
            }

            _poolEntities = new Untils.PoolObject<EntityView>(SpawnEntity, EntityInActive, EntityActive, 4);
        }

        public void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= Config.Interval)
            {
                _timer = 0f;

                if (Config.MaxCountEntity > Enemies.Count)
                {
                    _poolEntities.ReturnActive(1);
                }
            }

            if (_inActivation.Count > 0)
            {
                for (int i = 0; i < _inActivation.Count; i++)
                {
                    _poolEntities.ReturnInActive(_inActivation[i]);
                }
                _inActivation.Clear();
            }
        }

        private EntityView SpawnEntity()
        {
            var entityInstance = GameObject.Instantiate(Config.EntityPrefab, _unityEntity.ViewTransform.position, Quaternion.identity, _unityEntity.ViewTransform);
            entityInstance.SetActive(false);

            var view = entityInstance.GetComponent<EntityView>();
            view.Init(Config.EntityConfig);

            return view;
        }

        private void EntityInActive(EntityView entityView)
        {
            entityView.Entity.ViewTransform.gameObject.SetActive(false);
            entityView.Entity.GetSystem<EnemySuicideSystem>().OnDestroyed -= OnEntityDestroyed;
        }

        private void EntityActive(EntityView entityView)
        {
            entityView.Entity.ViewTransform.position = _unityEntity.ViewTransform.position;
            entityView.Entity.ViewTransform.rotation = _unityEntity.ViewTransform.rotation;

            var healthSystem = entityView.Entity.GetSystem<HealthSystem>();
            healthSystem.Resurrection();
            healthSystem.OnDeath += OnEntityDestroyed;

            entityView.Entity.GetSystem<EnemySuicideSystem>().OnDestroyed += OnEntityDestroyed;
            entityView.Entity.ViewTransform.gameObject.SetActive(true);
        }

        private void OnEntityDestroyed(UnityEntity entity)
        {
            var entityView = entity.ViewTransform.GetComponent<EntityView>();
            _inActivation.Add(entityView);
        }
    }
}
