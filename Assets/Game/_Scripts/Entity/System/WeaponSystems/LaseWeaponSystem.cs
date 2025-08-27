using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class LaserWeaponSystem : IWeaponSystem
    {
        private UnityEntity _unityEntity;
        private bool _hitCast;
        private RaycastHit _hitInfo;
        private Vector3 _hitPosition;
        private LineRenderer _beam;
        public Transform _muzzlePoint;

        public LaserWeaponSystemConfig Config { get; private set; }

        public LaserWeaponSystem(LaserWeaponSystemConfig config)
        {
            Config = config;
        }

        public void Init(Entity entity)
        {
            _unityEntity = entity as UnityEntity;
            _beam = _unityEntity.ViewTransform.GetComponent<LineRenderer>();
            _muzzlePoint = _unityEntity.ViewTransform;
        }

        public void Update()
        {

        }

        public void StartShoot()
        {
            _beam.enabled = true;
            Shooting();
        }

        public void StopShoot()
        {
            _beam.enabled = false;
        }

        private async Task Shooting()
        {
            while (_beam != null && _beam.enabled)
            {
                HitInfo();
                SetParametersVFX();

                if (_hitCast && _hitInfo.transform.TryGetComponent(out EntityView entity))
                {
                    var system = entity.Entity.GetSystem<HealthSystem>();
                    system?.TakeDamage(1);
                }

                await Task.Yield();
            }
        }

        private void SetParametersVFX()
        {
            _beam.SetPosition(0, _unityEntity.ViewTransform.position);
            _beam.SetPosition(1, _hitPosition);
        }

        protected void HitInfo()
        {
            var ray = new Ray(_muzzlePoint.position, _muzzlePoint.forward);
            _hitCast = Physics.Raycast(ray, out RaycastHit hitInfo, Config.MaxDistance, Config.LayerMask);
            _hitPosition = _hitCast ? hitInfo.point : _muzzlePoint.position + _muzzlePoint.forward * Config.MaxDistance;
            _hitInfo = hitInfo;
        }
    }
}
