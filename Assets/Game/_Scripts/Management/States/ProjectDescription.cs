using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Untils;

namespace Game
{
    public class ProjectDescription : IFSMState<StateGameplay>
    {
        private DIContainer _diContainer;

        private FSMGameplay _fSMGameplay;
        private ScreenView _projectDescriptionView;
        private UserInput _userInput;

        public ProjectDescription(DIContainer diContainer)
        {
            _diContainer = diContainer;

            _fSMGameplay = _diContainer.Resolve<FSMGameplay>();
            _projectDescriptionView = _diContainer.Resolve<ScreenView>("InfoProjectView");
            _userInput = _diContainer.Resolve<UserInput>();
        }

        public StateGameplay State => StateGameplay.ProjectDescription;

        public void Enter()
        {
            _userInput.ResumeEvent += Resume;
            _userInput.Restart += RstartGame;
            _userInput.OnUI();
            _projectDescriptionView.SetActive(true);
        }

        public void Exit()
        {
            _projectDescriptionView.SetActive(false);
        }

        private void RstartGame()
        {
            _userInput.Disposible();
            SceneManager.LoadScene("Gameplay");
        }

        private void Resume()
        {
            _userInput.ResumeEvent -= Resume;
            _userInput.Restart -= RstartGame;
            _userInput.Disable();

            _fSMGameplay.ExitAndResume();
        }
    }
}
