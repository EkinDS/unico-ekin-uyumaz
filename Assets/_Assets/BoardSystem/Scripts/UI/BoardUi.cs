using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Assets.BoardSystem
{
    public class BoardUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI instructionText;
        [SerializeField] private Board board;
        [SerializeField] private Button startLevelButton;
        [SerializeField] private TextMeshProUGUI currentLevelText;


        public void SwitchLevelButton(int levelId)
        {
            BoardEventsHandler.RaiseSwitchLevelButtonClicked(levelId);
        
            startLevelButton.gameObject.SetActive(true);
            currentLevelText.text = "Level " + levelId;
            currentLevelText.gameObject.SetActive(true);

            SetInstructionTextVisibility(true, "Place your defense items on the grids and prepare for battle!");
        }


        public void StartLevelButton()
        {
            BoardEventsHandler.RaiseStartEnemyAdvancesButtonClicked();

            startLevelButton.gameObject.SetActive(false);

            SetInstructionTextVisibility(false);
        }


        private void OnPlayerWon()
        {
            SetInstructionTextVisibility(true, "You won! Choose a level to start!");
        }


        private void OnPlayerFailed()
        {
            SetInstructionTextVisibility(true, "You failed! Choose a level to start.");
        }


        private void OnLevelStarted()
        {
            SetInstructionTextVisibility(false);
        }


        private void SetInstructionTextVisibility(bool isVisible, string text = "")
        {
            instructionText.gameObject.SetActive(isVisible);
            instructionText.text = text;
        }


        private void OnEnable()
        {
            BoardEventsHandler.PlayerWon += OnPlayerWon;
            BoardEventsHandler.PlayerFailed += OnPlayerFailed;
            BoardEventsHandler.LevelStarted += OnLevelStarted;
        }


        private void OnDisable()
        {
            BoardEventsHandler.PlayerWon -= OnPlayerWon;
            BoardEventsHandler.PlayerFailed -= OnPlayerFailed;
            BoardEventsHandler.LevelStarted -= OnLevelStarted;
        }
    }
}