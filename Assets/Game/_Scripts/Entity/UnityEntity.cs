using UnityEngine;

namespace Game
{
    public class UnityEntity : Entity
    {
        public Transform ViewTransform { get; private set; }

        public UnityEntity(Transform viewTransform, EntityConfig config)
        {
            ViewTransform = viewTransform;

            foreach (var systemConfig in config.Systems)
            {
                var system = systemConfig.CreateSystem();
                AddSystem(system);
            }
        }

        public T GetSystem<T>() where T : class, IEntitySystem
        {
            if (_systems.TryGetValue(typeof(T), out var system))
                return system as T;
            return null;
        }

        public TInterface GetSystemByInterface<TInterface>() where TInterface : class
        {
            foreach (var system in _systems.Values)
            {
                if (system is TInterface casted)
                    return casted;
            }
            return null;
        }
    }
}
