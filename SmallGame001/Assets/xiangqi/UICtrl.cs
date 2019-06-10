using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DXiangqi
{
    public class UICtrl : MonoBehaviour
    {
        public Image xuanze_red;
        public Image xuanze_black;

        public Text turnText;
        
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ChoosePosition(Camp camp,DPoint dPoint)
        {
            switch (camp)
            {
                case Camp.Red:
                    xuanze_red.transform.position = new Vector3(dPoint.X, dPoint.Y, 0);
                    break;
                case Camp.Black:
                    xuanze_black.transform.position = new Vector3(dPoint.X, dPoint.Y, 0);
                    break;
            }
        }

        public void ChangeRurn(Camp camp)
        {
            switch (camp)
            {
                case Camp.Red:
                    turnText.text = "轮到红棋走了";
                    xuanze_red.gameObject.SetActive(true);
                    xuanze_black.gameObject.SetActive(false);
                    break;
                case Camp.Black:
                    turnText.text = "轮到黑棋走了";
                    xuanze_red.gameObject.SetActive(false);
                    xuanze_black.gameObject.SetActive(true);
                    break;
            }
        }

        public void GameOver(Camp winner)
        {
            switch (winner)
            {
                case Camp.Red:
                    turnText.text = "红棋胜利！！！";
                    break;
                case Camp.Black:
                    turnText.text = "黑棋胜利！！！";
                    break;
            }
        }
    }
}