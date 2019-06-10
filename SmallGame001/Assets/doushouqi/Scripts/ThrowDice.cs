using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XDouShouQi
{
    /// <summary>
    /// 掷骰子
    /// </summary>
    public class ThrowDice : MonoBehaviour
    {
        public Transform dice;
        public Transform plane;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!isStart) return;

            if (timer < time)
            {
                dice.Rotate(new Vector3(dice.transform.rotation.x + rx, dice.transform.rotation.y + ry,
                                                    dice.transform.rotation.z + rz));
                timer += Time.deltaTime;
            }
            else
            {
                GetDiceCount();
            }
        }

        int diceCount = 0;
        float time;

        public void GetDiceCount()
        {
            if (Vector3.Dot(dice.transform.forward, Vector3.up) == 1)
                diceCount = 3;
            if (Vector3.Dot(dice.transform.forward, Vector3.up) == -1)
                diceCount = 4;
            if (Vector3.Dot(dice.transform.up, Vector3.up) == 1)
                diceCount = 1;
            if (Vector3.Dot(dice.transform.up, Vector3.up) == -1)
                diceCount = 6;
            if (Vector3.Dot(dice.transform.right, Vector3.up) == 1)
                diceCount = 5;
            if (Vector3.Dot(dice.transform.right, Vector3.up) == -1)
                diceCount = 2;

            if (diceCount > 0 && !stopped)
            {
                Debug.Log("DiceCount:" + diceCount);
                stopped = true;
                RollDiceEvent(diceCount);
            }
        }

        public void Show(bool b)
        {
            dice.gameObject.SetActive(b);
            plane.gameObject.SetActive(b);
        }

        float timer = 5; float rx, ry, rz;
        public void RotateDice(float second)
        {
            isStart = true;
            Show(true);

            rx = Random.Range(0, 180);
            ry = Random.Range(0, 180);
            rz = Random.Range(0, 180);
            timer = 0;
            time = second;
            stopped = false;
        }
        bool isStart = false;

        bool stopped = false;
        public delegate void RollDice(int dice);
        public event RollDice RollDiceEvent;
    }
}