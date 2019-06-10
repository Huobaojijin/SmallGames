using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XJump
{
    public class GameController : MonoSingleton<GameController>
    {
        public UIController _uiController;
        public FloorController _floorController;
        public PlayerController _playerController;

        public FollowTarget mainCamera;

        GameState gameState = GameState.Staying;

        public Text scores;

        // Use this for initialization
        void Start()
        {
            scores.text = "";
            GameStaying();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (gameState == GameState.Playing)
                {
                    GamePause();
                }
                else if (gameState == GameState.Pausing)
                {
                    GamePlay();
                }
                else { }
            }
        }

        public void GameStaying()
        {
            gameState = GameState.Staying;
            _uiController.GameStaying();

            _playerController.gameObject.SetActive(true);
            _playerController.PlayerStaying();

            _floorController.Staying();

            mainCamera.Staying();
            mainCamera.enabled = false;
        }

        public void GameStart()
        {
            gameState = GameState.Playing;
            _uiController.GameStart();
            _floorController.RandomSpawn();

            _playerController.gameObject.SetActive(true);
            _playerController.PlayerStart();

            mainCamera.enabled = true;
        }

        public void GameOver()
        {
            _uiController.GameOver();
            _floorController.Stop();

            _playerController.gameObject.SetActive(false);
        }

        public void GamePause()
        {
            gameState = GameState.Pausing;
            _uiController.GamePause(true);
            _floorController.Pause(true);
            Time.timeScale = 0;
        }

        public void GamePlay()
        {
            gameState = GameState.Playing;
            _uiController.GamePause(false);
            _floorController.Pause(false);
            Time.timeScale = 1;
        }

        public void GameRestart()
        {
            GameStaying();
            GameStart();
        }

        public void TotalScores(float dis)
        {
            scores.text = dis.ToString("F2");
        }
    }

    public enum GameState
    {
        Staying,
        Playing,
        Pausing,
        Over
    }
}