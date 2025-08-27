using System.Threading.Tasks;
using UnityEngine;
using Untils;

namespace Game
{
    public class FireWeaponSystem : IWeaponSystem
    {
        private UnityEntity _unityEntity;
        private bool _canShot = true;
        private bool _shooting = false;
        private PoolObject<Missile> _poolEntities;

        public FireWeaponSystemConfig Config { get; private set; }

        public FireWeaponSystem(FireWeaponSystemConfig config)
        {
            Config = config;
        }

        public void Init(Entity entity)
        {
            _unityEntity = entity as UnityEntity;
            _poolEntities = new PoolObject<Missile>(GenerationMissile, ReturnInActive, ReturnActive, 20);
        }

        public void Update()
        {

        }

        public void StartShoot()
        {
            _shooting = true;
            Shooting();
        }
        
        private async Task Shooting()
        {
            while (_shooting)
            {
                if (_canShot)
                {
                    _poolEntities.ReturnActive(1);
                    _canShot = false;
                }
                await Refresh();
            }
        }

        private async Task Refresh()
        {
            await Task.Delay(Config.CooldownTimeInMilliseconds);
            _canShot = true;
        }
        
        public void StopShoot()
        {
            _shooting = false;
        }

        private Missile GenerationMissile()
        {
            var missileObj = GameObject.Instantiate(Config.PrefabMissile, _unityEntity.ViewTransform.position, _unityEntity.ViewTransform.rotation);
            var missile = missileObj.GetComponent<Missile>();
            missile.Init(Config.Damage, Config.MaxDistance, MissileInActive);

            return missile;
        }

        private void MissileInActive(Missile missile)
        {
            _poolEntities.ReturnInActive(missile);
        }

        private void ReturnInActive(Missile missile)
        {
            missile.GetComponent<Rigidbody>().velocity = Vector3.zero;
            missile.gameObject.SetActive(false);
        }

        private void ReturnActive(Missile missile)
        {
            missile.transform.position = _unityEntity.ViewTransform.position;
            missile.transform.rotation = _unityEntity.ViewTransform.rotation;
            
            missile.gameObject.SetActive(true);
            missile.GetComponent<Rigidbody>().velocity = missile.transform.forward * Config.SpeedShot;
        }
    }
}
