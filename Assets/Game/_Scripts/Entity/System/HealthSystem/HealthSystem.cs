using System;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Config/Systems/Healt")]
    public class HealthSystem : IEntitySystem
    {
        private int _currentHealth;
        private HealthBarView _healthBar;
        private UnityEntity _unityEntity;

        public HealthSystemConfig Config { get; private set; }

        public HealthSystem(HealthSystemConfig config)
        {
            Config = config;
        }

        public event Action<UnityEntity> OnDeath;

        public void Init(Entity entity)
        {
            if (entity is UnityEntity unityEntity)
            {
                _unityEntity = unityEntity;

                var barInstance = GameObject.Instantiate(Config.HealthBarPreffab, _unityEntity.ViewTransform);
                barInstance.GetComponent<Canvas>().worldCamera = Camera.main;

                var barTransform = barInstance.GetComponent<RectTransform>();
                barTransform.position = _unityEntity.ViewTransform.position + Vector3.up * 2f;
                barTransform.LookAt(barTransform.position + Camera.main.transform.forward);

                _currentHealth = Config.MaxHealth;

                _healthBar = barInstance.GetComponent<HealthBarView>();
                _healthBar.Init(Config.MaxHealth);
            }
        }

        public void Update()
        {

        }

        public void TakeDamage(int damage)
        {
            _currentHealth = damage >= _currentHealth ? 0 : _currentHealth -= damage;
            _healthBar.UpdateView(_currentHealth);

            if (_currentHealth == 0)
            {
                OnDeath?.Invoke(_unityEntity);
            }
        }

        public void Resurrection()
        {
            _currentHealth = Config.MaxHealth;
            _healthBar.UpdateView(_currentHealth);
        }
    }
}
