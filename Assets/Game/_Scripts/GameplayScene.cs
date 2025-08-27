using System.Collections.Generic;
using UnityEngine;
using Untils;

namespace Game
{
    public class GameplayScene : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private EntityConfig _playerConfig;
        [SerializeField] private EntityConfig _fireWeaponConfig;
        [SerializeField] private EntityConfig _laserWeaponConfig;
        [SerializeField] private UserInput _userInput;

        [Header("Enemy")]
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private EntityConfig _enemyFollowConfig;

        [Header("Spawners")]
        [SerializeField] private List<EntityView> _entitySpawners = new();
        [SerializeField] private EntityConfig _spawnerConfig;

        [Header("UI")]
        [SerializeField] private ScreenView _projectDescriptionView;
        [SerializeField] private ScreenView _defeatView;

        public DIContainer _diContainer = new();

        private void Awake()
        {
            DIRegistrations();

            _diContainer.Resolve<FSMGameplay>().Init(new List<IFSMState<StateGameplay>>() {
                new Init(_diContainer),
                new MainLoop(_diContainer),
                new Pause(_diContainer),
                new ProjectDescription(_diContainer),
                new Defeat(_diContainer),
            });
        }

        private void Start()
        {
            _diContainer.Resolve<FSMGameplay>().EnterIn(StateGameplay.Init);
            _diContainer.Resolve<FSMGameplay>().EnterIn(StateGameplay.Pause);
        }

        private void DIRegistrations()
        {
            _diContainer.RegisterSingleton(_ => new FSMGameplay());

            _diContainer.RegisterInstance("EnemyFollowConfig", _enemyFollowConfig);
            _diContainer.RegisterInstance("PlayerConfig", _playerConfig);
            _diContainer.RegisterInstance("FireWeaponConfig", _fireWeaponConfig);
            _diContainer.RegisterInstance("LaserWeaponConfig", _laserWeaponConfig);
            _diContainer.RegisterInstance("SpawnerConfig", _spawnerConfig);

            _diContainer.RegisterInstance("EntityPrefab", _playerPrefab);

            _diContainer.RegisterInstance(_userInput);
            _diContainer.RegisterInstance(_entitySpawners);

            _diContainer.RegisterInstance("InfoProjectView", _projectDescriptionView);
            _diContainer.RegisterInstance("DefeatView", _defeatView);
        }
    }
}
