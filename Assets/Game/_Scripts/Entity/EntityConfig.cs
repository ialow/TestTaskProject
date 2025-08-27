using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "EntityConfig", menuName = "Config/Entity")]
    public class EntityConfig : ScriptableObject
    {
        [field: SerializeField] public EntitySystemConfig[] Systems { get; private set; }
        
        [Header("Children entities")]
        public List<ChildEntityConfig> Children;
    }

    [System.Serializable]
    public class ChildEntityConfig
    {
        public GameObject Prefab;
        public EntityConfig Config;
        public Vector3 LocalPosition;
        public Quaternion LocalRotation = Quaternion.identity;
    }
}
