using System;
using System.Collections.Generic;

namespace Game
{
    public abstract class Entity
    {
        protected Dictionary<Type, IEntitySystem> _systems = new();

        public virtual void AddSystem(IEntitySystem system)
        {
            _systems.Add(system.GetType(), system);
            system.Init(this);
        }

        public virtual void Update()
        {
            foreach (var system in _systems)
                system.Value.Update();
        }
    }
}
