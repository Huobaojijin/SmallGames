using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XJump
{
    public class UIController : MonoBehaviour
    {
        public GameObject mainUI;
        public GameObject playingUI;
        public GameObject pauseUI;
        public GameObject overUI;

        private void Start()
        {
            GameStaying();
        }

        public void GameStaying()
        {
            mainUI.SetActive(true);
            playingUI.SetActive(false);
            pauseUI.SetActive(false);
            overUI.SetActive(false);
        }

        public void GameStart()
        {
            mainUI.SetActive(false);
            playingUI.SetActive(true);
        }

        public void GamePause(bool b)
        {
            pauseUI.SetActive(b);
        }

        public void GameOver()
        {
            overUI.SetActive(true);
        }
    }
}