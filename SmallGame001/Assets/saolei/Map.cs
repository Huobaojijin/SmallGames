using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSaoLei
{
    /// <summary>
    /// 地图类，初级9*9*10，中级16*16*40，高级16*30*99
    /// </summary>
    public class Map : MonoBehaviour
    {
        public int Row;
        public int Col;
        public int lei;

        public GameObject unit;

        public GameObject line;

        private List<int> distribution = new List<int>();
        private List<Unit> distributionList = new List<Unit>();
        private List<Unit> Units = new List<Unit>();

        public void Init()
        {
            Units.Clear();
            distribution.Clear();
            distributionList.Clear();

            distribution = Shuffle();
            for (int i = 0; i < Row; ++i)
            {
                for (int j = 0; j < Col; ++j)
                {
                    GameObject g = Instantiate(unit, transform, false);
                    g.transform.localPosition = new Vector3(j, i, 0);
                    g.name = string.Format("unit{0}{1}", i, j);
                    g.GetComponent<Unit>().Row = i;
                    g.GetComponent<Unit>().Col = j;
                    if (distribution.Contains(i * Col + j))
                    {
                        g.GetComponent<Unit>().isLei = true;
                        distributionList.Add(g.GetComponent<Unit>());
                    }
                    Units.Add(g.GetComponent<Unit>());
                }
            }
        }

        private List<int> Shuffle()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < Row; ++i)
            {
                for (int j = 0; j<Col;++j)
                {
                    list.Add(i * Col + j);
                }
            }

            List<int> result = new List<int>();

            for(int i = 0; i < lei; ++i)
            {
                int rand = Random.Range(0, list.Count);
                result.Add(list[rand]);
                list.RemoveAt(rand);
            }

            return result;
        }

        public Unit GetUnit(int row, int col)
        {
            return Units[row * Col + col];
        }

        public int RoundLeiNumber(int row,int col)
        {
            int count = 0;

            for (int i = Mathf.Clamp(row - 1, 0, Row - 1); i <= Mathf.Clamp(row + 1, 0, Row - 1); i++)
            {
                for (int j = Mathf.Clamp(col - 1, 0, Col - 1); j <= Mathf.Clamp(col + 1, 0, Col - 1); j++)
                {
                    Unit unit = GetUnit(i, j);
                    if (unit.isLei) count++;
                }
            }
            if (GetUnit(row, col).isLei) count--;
            return count;
        }

        public void Clear()
        {
            for (int i = 0; i < Units.Count; i++)
            {
                Destroy(Units[i].gameObject);
            }
        }

        public void DrawLines()
        {
            for (int x = -15; x <= 15; x++)
            {
                GameObject l = Instantiate(line, new Vector3(x, 0, 0), Quaternion.identity);
                l.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, -9, 0));
                l.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 7, 0));
            }

            for (int y = -9; y <= 7; y++)
            {
                GameObject l = Instantiate(line, new Vector3(0, y, 0), Quaternion.identity);
                l.GetComponent<LineRenderer>().SetPosition(0, new Vector3(-15, 0, 0));
                l.GetComponent<LineRenderer>().SetPosition(1, new Vector3(15, 0, 0));
            }
        }

        public void ShowAllLeis()
        {
            for (int i = 0; i < distributionList.Count; ++i)
            {
                distributionList[i].HideBg();
                distributionList[i].GetComponent<TextMesh>().text = "雷";
            }
        }
    }
}