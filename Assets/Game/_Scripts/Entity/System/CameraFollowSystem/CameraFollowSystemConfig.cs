using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Config/Systems/CameraFollow")]
    public class CameraFollowSystemConfig : EntitySystemConfig
    {
        public float PositionLag = 2f;

        public override IEntitySystem CreateSystem()
        {
            return new CameraFollowSystem(this);
        }
    }
}
