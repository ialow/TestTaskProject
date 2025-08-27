using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Config/Systems/PlayerMovement")]
    public class PlayerMovementSystemConfig : EntitySystemConfig
    {
        public float Speed = 5f;
        public float TurnDegrees = 60f;
        public float IgnoringRadiusTurn = 4f;
        
        public UserInput Input;

        public override IEntitySystem CreateSystem()
        {
            return new PlayerMovementSystem(this);
        }
    }
}
