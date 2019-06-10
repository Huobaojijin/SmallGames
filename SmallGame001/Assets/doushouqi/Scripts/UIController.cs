using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XDouShouQi
{
    public class UIController : MonoBehaviour
    {
        public Transform turnImg;
        public Transform hongText;
        public Transform lanText;

        public GameObject gameOverUI;
        public Text winnerText;

        public GameObject gameStartUI;
        // Use this for initialization
        void Start()
        {
            gameStartUI.SetActive(true);
            gameOverUI.SetActive(false);
        }

        public void GameStart()
        {
            gameStartUI.SetActive(false);
            gameOverUI.SetActive(false);
        }

        public void GameOver(Winner winner)
        {
            gameOverUI.SetActive(true);
            string temp = "";
            winnerText.text = "";
            switch (winner)
            {
                case Winner.Hong:
                    temp = "红方胜！";
                    break;
                case Winner.Lan:
                    temp = "蓝方胜！";
                    break;
                case Winner.Deuce:
                    temp = "平局！";
                    break;
            }
            winnerText.text = temp;
        }

        public void ChooseTurn(Camp camp)
        {
            switch (camp)
            {
                case Camp.Lan:
                    turnImg.position = lanText.position;
                    break;
                case Camp.Hong:
                    turnImg.position = hongText.position;
                    break;
            }
        }
    }
}