using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Untils;

namespace Game
{
    public class MainLoop : ISuspendFSMState<StateGameplay>
    {
        private DIContainer _diContainer;

        private FSMGameplay _fSMGameplay;
        private CancellationTokenSource _tokenPuase;
        private UserInput _userInput;
        private List<EntityView> _entitySpawners;

        public MainLoop(DIContainer diContainer)
        {
            _diContainer = diContainer;

            _fSMGameplay = _diContainer.Resolve<FSMGameplay>();
            _userInput = _diContainer.Resolve<UserInput>();
            _entitySpawners = _diContainer.Resolve<List<EntityView>>();
        }

        public StateGameplay State => StateGameplay.MainLoop;

        public void Enter()
        {
            _userInput.Enable().OnGameplay();
            _userInput.PuaseEvent += Pause;

            _tokenPuase = new();
            RunLoop();
        }

        public void Exit()
        {
            _tokenPuase.Cancel();
            EntitySystemPause();
            _userInput.PuaseEvent -= Pause;
        }

        private async Task RunLoop()
        {
            var playerView = _diContainer.Resolve<EntityView>("Player");

            while (!_tokenPuase.IsCancellationRequested)
            {
                for (var i = 0; i < _entitySpawners.Count; i++)
                {
                    _entitySpawners[i].Entity.Update();
                    
                    var enemys = _entitySpawners[i].Entity.GetSystem<SpawnerSystem>().Enemies;
                    for (var j = 0; j < enemys.Count; j++)
                    {
                        enemys[j].Entity.Update();
                    }
                }

                playerView.Entity.Update();
                await Task.Yield();
            }
        }

        private void Pause()
        {
            _fSMGameplay.SuspendAndEnterIn(StateGameplay.Pause);
        }

        private void EntitySystemPause()
        {
            for (var i = 0; i < _entitySpawners.Count; i++)
            {
                var enemys = _entitySpawners[i].Entity.GetSystem<SpawnerSystem>().Enemies;
                for (var j = 0; j < enemys.Count; j++)
                {
                    enemys[j].Entity.GetSystem<EnemyFollowSystem>().OnPause();
                }
            }
        }

        public void Suspend()
        {
            EntitySystemPause();
            _userInput.PuaseEvent -= Pause;
            _userInput.Disable();

            _tokenPuase.Cancel();
        }

        public void Resume()
        {
            _userInput.PuaseEvent += Pause;
            _userInput.OnGameplay();
            _tokenPuase = new();
        }
    }
}
