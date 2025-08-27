using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Untils;

namespace Game
{
    public class Init : IFSMState<StateGameplay>
    {
        private DIContainer _diContainer;
        private FSMGameplay _fSMGameplay;

        public Init(DIContainer diContainer)
        {
            _diContainer = diContainer;
            _fSMGameplay = _diContainer.Resolve<FSMGameplay>();
        }

        public StateGameplay State => StateGameplay.Init;

        public void Enter()
        {
            var playerPrefab = _diContainer.Resolve<GameObject>("EntityPrefab");
            var playerConfig = _diContainer.Resolve<EntityConfig>("PlayerConfig");

            var playerView = CreaterPlayerWithWeapon(new Vector3(0, 1f, 0), playerPrefab, playerConfig);
            playerView.tag = "Player";
            playerView.Entity.GetSystem<HealthSystem>().OnDeath += PlayerDeath;

            _diContainer.RegisterInstance("Player", playerView);

            var spawnerConfig = _diContainer.Resolve<EntityConfig>("SpawnerConfig");
            _diContainer.Resolve<List<EntityView>>().ForEach(x => x.Init(spawnerConfig));

            _fSMGameplay.EnterIn(StateGameplay.MainLoop);
        }

        private EntityView CreaterPlayerWithWeapon(Vector3 position, GameObject prefab, EntityConfig config)
        {
            var playerInstance = GameObject.Instantiate(prefab, position, Quaternion.identity);
            var view = playerInstance.GetComponent<EntityView>();

            view.Init(config);
            var weaponSystem = view.Entity.GetSystem<WeaponSystem>();

            foreach (var childerConfig in config.Children)
            {
                var childerInstance = GameObject.Instantiate(childerConfig.Prefab, childerConfig.LocalPosition, childerConfig.LocalRotation, playerInstance.transform);
                var childerView = childerInstance.GetComponent<EntityView>();
                childerView.Init(childerConfig.Config);
                weaponSystem.AddWeapon(childerView.Entity);
            }

            return view;
        }

        private void PlayerDeath(UnityEntity unityEntity)
        {
            _fSMGameplay.EnterIn(StateGameplay.Defeat);
        }

        public void Exit()
        {

        }
    }
}
