using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Config/Systems/MovementFollow")]
    public class EnemyFollowSystemConfig : EntitySystemConfig
    {
        public float Speed = 3f;
        public float StoppingDistance = 1f;

        public override IEntitySystem CreateSystem()
        {
            return new EnemyFollowSystem(this);
        }
    }
}
