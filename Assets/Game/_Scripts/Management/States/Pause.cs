using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Untils;

namespace Game
{
    public class Pause : ISuspendFSMState<StateGameplay>
    {
        private DIContainer _diContainer;

        private FSMGameplay _fSMGameplay;

        public Pause(DIContainer diContainer)
        {
            _diContainer = diContainer;
            _fSMGameplay = _diContainer.Resolve<FSMGameplay>();
        }

        public StateGameplay State => StateGameplay.Pause;

        public void Enter()
        {
            _fSMGameplay.SuspendAndEnterIn(StateGameplay.ProjectDescription);
        }

        public void Exit()
        {

        }

        public void Resume()
        {
            _fSMGameplay.EnterIn(StateGameplay.MainLoop);
        }

        public void Suspend()
        {
            
        }
    }
}
