using System;
using UnityEngine;

namespace Game
{
    public class EntityView : MonoBehaviour
    {
        public UnityEntity Entity { get; private set; }

        public void Init(EntityConfig config)
        {
            Entity = new UnityEntity(transform, config);
        }
    }
}
