using System;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Config/Systems/SuicideSystem")]
    public class EnemySuicideSystemConfig : EntitySystemConfig
    {
        public int Damage = 30;
        public GameObject ExplosionPrefab;

        public override IEntitySystem CreateSystem()
        {
            return new EnemySuicideSystem(this);
        }
    }
}
