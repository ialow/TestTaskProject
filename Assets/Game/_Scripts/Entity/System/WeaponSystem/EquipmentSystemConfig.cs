using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Config/Systems/Weapon")]
    public class EquipmentSystemConfig : EntitySystemConfig
    {
        public UserInput UserInput;
        
        public override IEntitySystem CreateSystem()
        {
            return new WeaponSystem(this);
        }
    }
}
