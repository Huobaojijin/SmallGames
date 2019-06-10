using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace X2048
{
    public class GameCtrl : MonoBehaviour
    {
        public UICtrl UICtrl;

        public GameObject[] prefabs;

        List<List<Cell>> cells;

        // Use this for initialization
        void Start()
        {
            cells = new List<List<Cell>>();
            for (int row = 0; row < 4; ++row)
            {
                List<Cell> list = new List<Cell>();
                for (int col = 0; col < 4; ++col)
                {
                    Cell cell = new Cell();
                    cell.Transform = transform.Find(string.Format("grid{0}{1}", row, col));
                    list.Add(cell);
                }
                cells.Add(list);
            }

            Init();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Init()
        {
            UICtrl.GameStart();

            for (int i = 0; i < 2; i++)
            {
                InstantiateOne();
            }
        }

        private bool InstantiateOne()
        {
            List<Cell> emptyCells = new List<Cell>();
            for (int row = 0; row < 4; ++row)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (cells[row][col].Son == null)
                    {
                        emptyCells.Add(cells[row][col]);
                    }
                }
            }

            if (emptyCells.Count == 0)
            {
                return false;
            }

            int rand = Random.Range(0, emptyCells.Count);
            Cell cell = emptyCells[rand];

            GameObject g = Instantiate(prefabs[0]);
            g.GetComponent<Item>().isMerge = true;
            g.transform.parent = cell.Transform;
            g.transform.localPosition = Vector3.zero;
            cell.Score = 2;
            cell.Son = g;

            return true;
        }

        private void ResetMerge()
        {
            Item[] items = FindObjectsOfType<Item>();
            foreach (Item item in items)
            {
                item.isMerge = true;
            }
        }

        public void MoveGrid(Direction direction)
        {
            ResetMerge();

            switch (direction)
            {
                case Direction.LEFT:
                    MoveLeft();
                    break;
                case Direction.RIGHT:
                    MoveRight();
                    break;
                case Direction.DOWN:
                    MoveDown();
                    break;
                case Direction.UP:
                    MoveUp();
                    break;
            }
            InstantiateOne();

            Fail();
        }
        
        private void MoveLeft()
        {
            for (int row = 0; row < 4; ++row)
            {
                for (int col = 1; col < 4; ++col)
                {
                    Cell cell = cells[row][col];
                    if (cell.Son != null)
                    {
                        bool flag = false;//判断是否有过合并
                        for (int i = col - 1; i >= 0; --i)
                        {
                            Cell temp = cells[row][i];
                            if (temp.Son != null)//左边存在一个格子有数字
                            {
                                Item cellItem = cell.Son.GetComponent<Item>();
                                Item tempItem = temp.Son.GetComponent<Item>();
                                if (tempItem.isMerge && cellItem.isMerge && cellItem.score == tempItem.score)//能够合并
                                {
                                    Destroy(cell.Son);
                                    cell.Son = null;
                                    Destroy(temp.Son);
                                    temp.Son = null;

                                    /*合并，在格子上重新生成一个实例，分数为合并数的2倍*/
                                    GameObject g = Instantiate(prefabs[(int)Mathf.Log(cell.Score, 2)]);
                                    g.GetComponent<Item>().isMerge = false;
                                    g.transform.parent = temp.Transform;
                                    g.transform.localPosition = Vector3.zero;
                                    temp.Score *= 2;
                                    temp.Son = g;
                                    flag = true;

                                    Scoring(temp.Score);//计分
                                }
                                break;//找到一个非空格就终止查询
                            }
                        }

                        if (!flag)
                        {
                            int index = col;
                            for (int i = col - 1; i >= 0; --i)
                            {
                                Cell temp = cells[row][i];
                                if (temp.Son == null)//左边有一个空格子
                                {
                                    index--;//找到最近的空格
                                }
                                else
                                {
                                    break;//发现不是空位就终止查询
                                }
                            }
                            if (index < col)
                            {
                                Destroy(cell.Son);
                                cell.Son = null;

                                GameObject son = Instantiate(prefabs[(int)Mathf.Log(cell.Score, 2) - 1]);
                                son.GetComponent<Item>().isMerge = true;
                                Cell cellnew = cells[row][index];
                                son.transform.parent = cellnew.Transform;
                                son.transform.localPosition = Vector3.zero;
                                cellnew.Score = cell.Score;
                                cellnew.Son = son;
                            }
                        }
                    }
                }
            }
        }

        private void MoveRight()
        {
            for (int row = 0; row < 4; ++row)
            {
                for (int col = 2; col >= 0; --col)
                {
                    Cell cell = cells[row][col];
                    if (cell.Son != null)
                    {
                        bool flag = false;
                        for (int i = col + 1; i < 4; ++i)
                        {
                            Cell temp = cells[row][i];
                            if (temp.Son != null)//右边存在一个格子有数字
                            {
                                Item cellItem = cell.Son.GetComponent<Item>();
                                Item tempItem = temp.Son.GetComponent<Item>();
                                if (tempItem.isMerge && cellItem.isMerge && cellItem.score == tempItem.score)
                                {
                                    Destroy(cell.Son);
                                    cell.Son = null;
                                    Destroy(temp.Son);
                                    temp.Son = null;

                                    GameObject g = Instantiate(prefabs[(int)Mathf.Log(cell.Score, 2)]);
                                    g.GetComponent<Item>().isMerge = false;
                                    g.transform.parent = temp.Transform;
                                    g.transform.localPosition = Vector3.zero;
                                    temp.Score *= 2;
                                    temp.Son = g;
                                    flag = true;

                                    Scoring(temp.Score);//计分
                                }
                                break;//找到一个非空格就终止查询
                            }
                        }

                        if (!flag)
                        {
                            int index = col;
                            for (int i = col + 1; i < 4; ++i)
                            {
                                Cell temp = cells[row][i];
                                if (temp.Son == null)//右边有一个空格子
                                {
                                    index = i;//找到最近的空格
                                }
                                else
                                {
                                    break;//发现不是空位就终止查询
                                }
                            }
                            if (index > col)
                            {
                                Destroy(cell.Son);
                                cell.Son = null;

                                GameObject son = Instantiate(prefabs[(int)Mathf.Log(cell.Score, 2) - 1]);
                                son.GetComponent<Item>().isMerge = true;
                                Cell cellnew = cells[row][index];
                                son.transform.parent = cellnew.Transform;
                                son.transform.localPosition = Vector3.zero;
                                cellnew.Score = cell.Score;
                                cellnew.Son = son;
                            }
                        }
                    }
                }
            }
        }

        private void MoveUp()
        {
            for (int col = 0; col < 4; ++col)
            {
                for (int row = 1; row < 4; ++row)
                {
                    Cell cell = cells[row][col];
                    if (cell.Son != null)
                    {
                        bool flag = false;
                        for (int i = row - 1; i >= 0; --i)
                        {
                            Cell temp = cells[i][col];
                            if (temp.Son != null)//上边存在一个格子有数字
                            {
                                Item cellItem = cell.Son.GetComponent<Item>();
                                Item tempItem = temp.Son.GetComponent<Item>();
                                if (tempItem.isMerge && cellItem.isMerge && cellItem.score == tempItem.score)
                                {
                                    Destroy(cell.Son);
                                    cell.Son = null;
                                    Destroy(temp.Son);
                                    temp.Son = null;

                                    GameObject g = Instantiate(prefabs[(int)Mathf.Log(cell.Score, 2)]);
                                    g.GetComponent<Item>().isMerge = false;
                                    g.transform.parent = temp.Transform;
                                    g.transform.localPosition = Vector3.zero;
                                    temp.Score *= 2;
                                    temp.Son = g;
                                    flag = true;

                                    Scoring(temp.Score);//计分
                                }
                                break;//找到一个非空格就终止查询
                            }
                        }

                        if (!flag)
                        {
                            int index = row;
                            for (int i = row - 1; i >= 0; --i)
                            {
                                Cell temp = cells[i][col];
                                if (temp.Son == null)//上边有一个空格子
                                {
                                    index = i;//找到最近的空格
                                }
                                else
                                {
                                    break;//发现不是空位就终止查询
                                }
                            }
                            if (index < row)
                            {
                                Destroy(cell.Son);
                                cell.Son = null;

                                GameObject son = Instantiate(prefabs[(int)Mathf.Log(cell.Score, 2) - 1]);
                                son.GetComponent<Item>().isMerge = true;
                                Cell cellnew = cells[index][col];
                                son.transform.parent = cellnew.Transform;
                                son.transform.localPosition = Vector3.zero;
                                cellnew.Score = cell.Score;
                                cellnew.Son = son;
                            }
                        }
                    }
                }
            }
        }

        private void MoveDown()
        {
            for (int col = 0; col < 4; ++col)
            {
                for (int row = 2; row >= 0; --row)
                {
                    Cell cell = cells[row][col];
                    if (cell.Son != null)
                    {
                        bool flag = false;
                        for (int i = row + 1; i < 4; ++i)
                        {
                            Cell temp = cells[i][col];
                            if (temp.Son != null)//左边存在一个格子有数字
                            {
                                Item cellItem = cell.Son.GetComponent<Item>();
                                Item tempItem = temp.Son.GetComponent<Item>();
                                if (tempItem.isMerge && cellItem.isMerge && cellItem.score == tempItem.score)
                                {
                                    Destroy(cell.Son);
                                    cell.Son = null;
                                    Destroy(temp.Son);
                                    temp.Son = null;

                                    GameObject g = Instantiate(prefabs[(int)Mathf.Log(cell.Score, 2)]);
                                    g.GetComponent<Item>().isMerge = false;
                                    g.transform.parent = temp.Transform;
                                    g.transform.localPosition = Vector3.zero;
                                    temp.Score *= 2;
                                    temp.Son = g;
                                    flag = true;

                                    Scoring(temp.Score);//计分
                                }
                                break;//找到一个非空格就终止查询
                            }
                        }

                        if (!flag)
                        {
                            int index = row;
                            for (int i = row + 1; i < 4; ++i)
                            {
                                Cell temp = cells[i][col];
                                if (temp.Son == null)//下边有一个空格子
                                {
                                    index = i;//找到最近的空格
                                }
                                else
                                {
                                    break;//发现不是空位就终止查询
                                }
                            }
                            if (index > row)
                            {
                                Destroy(cell.Son);
                                cell.Son = null;

                                GameObject son = Instantiate(prefabs[(int)Mathf.Log(cell.Score, 2) - 1]);
                                son.GetComponent<Item>().isMerge = true;
                                Cell cellnew = cells[index][col];
                                son.transform.parent = cellnew.Transform;
                                son.transform.localPosition = Vector3.zero;
                                cellnew.Score = cell.Score;
                                cellnew.Son = son;
                            }
                        }
                    }
                }
            }
        }

        /*判断游戏结束*/
        private bool Fail()
        {
            for (int row = 0; row < 4; ++row)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (cells[row][col].Son == null)
                    {
                        return false;
                    }
                }
            }

            for (int row = 0; row < 4; ++row)
            {
                for (int col = 0 ; col < 3; ++col)
                {
                    if (cells[row][col].Score == cells[row][col+1].Score)
                    {
                        return false;
                    }
                }
            }

            for (int row = 0; row < 3; ++row)
            {
                for (int col = 0; col < 4; ++col)
                {
                    if (cells[row][col].Score == cells[row + 1][col].Score)
                    {
                        return false;
                    }
                }
            }

            GameOver();
            return true;
        }

        private int score = 0;
        private void Scoring(int s)
        {
            score += s;
            UICtrl.TotalScores(score);
        }

        private void GameOver()
        {
            Debug.Log("GameOver");
            UICtrl.GameOver();
        }

        /// <summary>
        /// 重新开始
        /// </summary>
        public void Restart()
        {
            for (int row = 0; row < 4; ++row)
            {
                for (int col = 0; col < 4; ++col)
                {
                    Cell cell = cells[row][col];
                    Destroy(cell.Son);
                    cell.Son = null;
                }
            }

            Init();
        }
    }

    public class Cell
    {
        public Transform Transform { get; set; }

        public int Score { get; set; }

        public GameObject Son { get; set; }
    }
}