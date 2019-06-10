using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace X2048
{
    public class UICtrl : MonoBehaviour
    {
        public GameObject gameoverCanvas;
        public Text scores;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void GameStart()
        {
            TotalScores(0);
            gameoverCanvas.SetActive(false);
        }

        public void TotalScores(int score)
        {
            scores.text = string.Format("{0}", score);
        }

        public void GameOver()
        {
            gameoverCanvas.SetActive(true);
        }
    }
}