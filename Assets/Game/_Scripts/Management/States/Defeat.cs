using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Untils;

namespace Game
{
    public class Defeat : IFSMState<StateGameplay>
    {
        private DIContainer _diContainer;
        private UserInput _userInput;

        public Defeat(DIContainer diContainer)
        {
            _diContainer = diContainer;
            _userInput = _diContainer.Resolve<UserInput>();
        }

        public StateGameplay State => StateGameplay.Defeat;

        public void Enter()
        {
            _userInput.Restart += RstartGame;
            _userInput.OnUI();
            
            var view = _diContainer.Resolve<ScreenView>("DefeatView");
            view.gameObject.SetActive(true);
        }

        public void Exit()
        {

        }

        private void RstartGame()
        {
            _userInput.Disposible();
            SceneManager.LoadScene("Gameplay");
        }
    }
}
