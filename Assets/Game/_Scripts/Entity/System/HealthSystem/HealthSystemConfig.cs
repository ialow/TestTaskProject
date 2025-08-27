using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Config/Systems/Healt")]
    public class HealthSystemConfig : EntitySystemConfig
    {
        public int MaxHealth = 100;
        public GameObject HealthBarPreffab;

        public override IEntitySystem CreateSystem()
        {
            return new HealthSystem(this);
        }
    }
}
