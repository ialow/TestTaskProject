using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Config/Systems/LaserWeapon")]
    public class LaserWeaponSystemConfig : EntitySystemConfig
    {
        public float MaxDistance = 20f;
        public LayerMask LayerMask;

        public override IEntitySystem CreateSystem()
        {
            return new LaserWeaponSystem(this);
        }
    }
}
