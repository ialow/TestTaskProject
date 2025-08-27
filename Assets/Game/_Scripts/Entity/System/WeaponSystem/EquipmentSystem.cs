using System.Collections.Generic;

namespace Game
{
    public class WeaponSystem : IEntitySystem
    {
        private List<UnityEntity> _entityWeapons = new();
        private int _numpadKey = 1;

        public EquipmentSystemConfig Config { get; private set; }

        public WeaponSystem(EquipmentSystemConfig config)
        {
            Config = config;
        }

        public void Init(Entity entity)
        {
            Config.UserInput.ToolbarEvent += SwitchWeapon;
            
            Config.UserInput.PerformedUseItemEvent += StartShoot;
            Config.UserInput.CanceledUseItemEvent += StopShoot;
        }

        public void Update()
        {

        }
        public void AddWeapon(UnityEntity weaponEntity)
        {
            _entityWeapons.Add(weaponEntity);
        }

        private void SwitchWeapon(int numpadKey)
        {
            StopShoot();
            _numpadKey = numpadKey;
        }

        private void StartShoot()
        {
            if (_entityWeapons.Count <= _numpadKey - 1)
                return;

            var weapon = _entityWeapons[_numpadKey - 1].GetSystemByInterface<IWeaponSystem>();
            weapon.StartShoot();
        }

        private void StopShoot()
        {
            if (_entityWeapons.Count <= _numpadKey - 1)
                return;

            var weapon = _entityWeapons[_numpadKey - 1].GetSystemByInterface<IWeaponSystem>();
            weapon.StopShoot();
        }
    }
}
