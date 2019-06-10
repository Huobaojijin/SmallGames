using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XDouShouQi
{
    public class GameControl : MonoBehaviour
    {
        public List<GameObject> animalPrefabs;

        public Camp currentCamp = Camp.Hong;

        public PlayerController h_playerController;
        public PlayerController l_playerController;

        public UIController uiController;

        public ThrowDice throwDice;

        private Cell[] cells;
        private List<GameObject> animals;
        // Use this for initialization
        void Start()
        {
            cells = transform.GetComponentsInChildren<Cell>();
            animals = new List<GameObject>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            h_playerController.enabled = false;
            l_playerController.enabled = false;

            h_playerController.TurnEndEvent += ChangeTurn;
            h_playerController.BattleProcessEvent += Statistic;

            l_playerController.TurnEndEvent += ChangeTurn;
            l_playerController.BattleProcessEvent += Statistic;

            throwDice.RollDiceEvent += GetTurn;
            DecideTurn();

            hongAnimalsCount = 8;
            lanAnimalsCount = 8;
            turnNumber = 0;

            uiController.GameStart();

            animals.Clear();

            int[] order = Shuffle();
            for (int i = 0; i < order.Length; i++)
            {
                int index = order[i];
                GameObject g = Instantiate(animalPrefabs[index], Vector3.zero, Quaternion.identity);
                animals.Add(g);
                if (i < order.Length / 2)
                {
                    g.transform.parent = cells[i].transform;
                    g.transform.localPosition = Vector3.zero;
                    g.transform.localRotation = Quaternion.Euler(0, 180, 0);
                    cells[i].son = g;
                }
                else
                {
                    g.transform.parent = cells[i + 4].transform;
                    g.transform.localPosition = Vector3.zero;
                    g.transform.localRotation = Quaternion.Euler(0, 180, 0);
                    cells[i + 4].son = g;
                }
            }
        }

        private void DecideTurn()
        {
            throwDice.RotateDice(1);
        }

        private void GetTurn(int num)
        {
            StartCoroutine("DiceAnimation", num);
        }

        private IEnumerator DiceAnimation(int num)
        {
            yield return new WaitForSeconds(2);
            bool temp = num < 4;
            if (temp)
            {
                currentCamp = Camp.Hong;
                h_playerController.enabled = true;
            }
            else
            {
                currentCamp = Camp.Lan;
                l_playerController.enabled = true;
            }
            uiController.ChooseTurn(currentCamp);
            throwDice.Show(false);
        }

        /// <summary>
        /// 洗牌
        /// </summary>
        /// <returns></returns>
        private int[] Shuffle()
        {
            int[] cards = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            for (int i = cards.Length-1; i >= 0; --i)
            {
                int r = Random.Range(0, i+1);
                int temp = cards[r];
                cards[r] = cards[i];
                cards[i] = temp;
            }
            return cards;
        }

        /// <summary>
        /// 切换回合
        /// </summary>
        /// <param name="turn"></param>
        private void ChangeTurn(Camp turn)
        {
            turnNumber++;

            bool result = JudgeWinner();
            if (result)
            {
                GameOver();
                return;
            }

            switch (turn)
            {
                case Camp.Hong:
                    h_playerController.enabled = false;
                    l_playerController.enabled = true;
                    currentCamp = Camp.Lan;
                    break;
                case Camp.Lan:
                    h_playerController.enabled = true;
                    l_playerController.enabled = false;
                    currentCamp = Camp.Hong;
                    break;
            }

            uiController.ChooseTurn(currentCamp);
        }

        /// <summary>
        /// 结束
        /// </summary>
        private void GameOver()
        {
            h_playerController.enabled = false;
            l_playerController.enabled = false;

            h_playerController.TurnEndEvent -= ChangeTurn;
            h_playerController.BattleProcessEvent -= Statistic;

            l_playerController.TurnEndEvent -= ChangeTurn;
            l_playerController.BattleProcessEvent -= Statistic;

            throwDice.RollDiceEvent -= GetTurn;

            uiController.GameOver(winner);

            for (int i = 0; i < animals.Count; ++i)
            {
                Destroy(animals[i]);
            }
        }

        private bool JudgeWinner() 
        {
            if (hongAnimalsCount == 0 && lanAnimalsCount != 0)
            {
                winner = Winner.Lan;
                return true;
            }
            if (hongAnimalsCount != 0 && lanAnimalsCount == 0)
            {
                winner = Winner.Hong;
                return true;
            }

            if (hongAnimalsCount == 0 && lanAnimalsCount == 0)
            {
                winner = Winner.Deuce;
                return true;
            }

            if (turnNumber >= 100)
            {
                winner = Winner.Deuce;
                return true;
            }

            return false;
        }

        public void Statistic(int hong,int lan)
        {
            hongAnimalsCount -= hong;
            lanAnimalsCount -= lan;
        }

        int hongAnimalsCount = 0;
        int lanAnimalsCount = 0;
        private Winner winner;
        private int turnNumber = 0;
    }

    public enum Winner
    {
        Hong,
        Lan,
        Deuce
    }
}