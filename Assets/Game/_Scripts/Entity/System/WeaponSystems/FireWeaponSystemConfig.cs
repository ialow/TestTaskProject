using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Config/Systems/FireWeapon")]
    public class FireWeaponSystemConfig : EntitySystemConfig
    {
        public int Damage;
        public float MaxDistance;
        public float SpeedShot;
        public int CooldownTimeInMilliseconds;
        public GameObject PrefabMissile;
        
        public override IEntitySystem CreateSystem()
        {
            return new FireWeaponSystem(this);
        }
    }
}
