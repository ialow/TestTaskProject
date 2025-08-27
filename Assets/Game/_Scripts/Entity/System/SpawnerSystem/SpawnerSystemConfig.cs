using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Config/Systems/Spawner")]
    public class SpawnerSystemConfig : EntitySystemConfig
    {
        public GameObject EntityPrefab;
        public EntityConfig EntityConfig;
        public float Interval = 2f;
        public int MaxCountEntity = 2;

        public override IEntitySystem CreateSystem()
        {
            return new SpawnerSystem(this);
        }
    }
}
