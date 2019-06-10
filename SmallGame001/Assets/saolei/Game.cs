using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DSaoLei
{
    public class Game : MonoBehaviour
    {
        public Text tipText;

        private bool isOver = false;
        private Map Map;
        private int leftLeis = 0;
        private int leftUnShowedUnits = 0;

        // Use this for initialization
        void Start()
        {
            Map = GetComponent<Map>();
            Map.DrawLines();

            NewGame();
        }

        // Update is called once per frame
        void Update()
        {
            if (isOver) return;

            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Unit _unit = hit.collider.GetComponent<Unit>();
                    if (_unit != null)
                    {
                        ClickUnit(_unit);
                    }
                }
            }

            //右键标记
            if (Input.GetMouseButtonUp(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Unit unit = hit.collider.GetComponent<Unit>();
                    if (unit != null)
                    {
                        if (unit.isShowed) return;
                        switch(unit.UnitState){
                            case UnitState.Empty:
                                unit.UnitState = UnitState.Flag;
                                unit.GetComponent<TextMesh>().text = "!";
                                leftLeis--;
                                break;
                            case UnitState.Flag:
                                unit.UnitState = UnitState.Doubt;
                                unit.GetComponent<TextMesh>().text = "?";
                                leftLeis++;
                                break;
                            case UnitState.Doubt:
                                unit.UnitState = UnitState.Empty;
                                unit.GetComponent<TextMesh>().text = "";
                                break;
                        }
                        UITips();
                    }
                }
            }

            GameWin();
        }

        public void ClickUnit(Unit unit)
        {
            if (unit.isShowed || unit.UnitState == UnitState.Flag) return;
            unit.isShowed = true;
            leftUnShowedUnits--;

            if (unit.isLei)
            {
                GameFail();//踩到地雷，游戏失败
                return;
            }

            int count = Map.RoundLeiNumber(unit.Row, unit.Col);
            unit.HideBg();
            if (count != 0)
            {
                unit.GetComponent<TextMesh>().text = count.ToString();
                unit.GetComponent<TextMesh>().color = DifferentColor(count);
            }
            else//连锁查询，找到连接的所有周围无雷格子的绝对空地
            {
                for (int i = Mathf.Clamp(unit.Row - 1, 0, Map.Row - 1); i <= Mathf.Clamp(unit.Row + 1, 0, Map.Row - 1); ++i)
                {
                    for (int j = Mathf.Clamp(unit.Col - 1, 0, Map.Col - 1); j <= Mathf.Clamp(unit.Col + 1, 0, Map.Col - 1); ++j)
                    {
                        Unit nearunit = Map.GetUnit(i, j);
                        ClickUnit(nearunit);
                    }
                }
            }
        }

        public void NewGame()
        {
            Map.Clear();
            Map.Init();
            isOver = false;
            leftLeis = Map.lei;
            leftUnShowedUnits = Map.Row * Map.Col;
            UITips();
        }

        public void UITips()
        {
            tipText.text = "剩余雷数：" + leftLeis;
        }

        private void GameFail()
        {
            isOver = true;//失败，结束
            Map.ShowAllLeis();
        }

        private void GameWin()
        {
            if (leftLeis == 0 && leftUnShowedUnits == Map.lei)
            {
                Debug.Log("Win");
                isOver = true;
            }
        }

        private Color DifferentColor(int num)
        {
            Color color = Color.white;
            switch (num)
            {
                case -1://雷
                    color = Color.red;
                    break;
                case 1://雷
                    color = Color.blue;
                    break;
                case 2://雷
                    color = new Color(0, 0, 0.8f);
                    break;
                case 3://雷
                    color = Color.red;
                    break;
                case 4://雷
                    color = new Color(0, 0, 0.5f);
                    break;
                case 5://雷
                    color = new Color(0, 0.3f, 0);
                    break;
                case 6://雷
                    color = new Color(0.4f, 0.4f, 0);
                    break;
                case 7://雷
                    color = Color.red;
                    break;
                case 8://雷
                    color = Color.red;
                    break;
            }
            return color;
        }
    }
}