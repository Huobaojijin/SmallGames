using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DXiangqi
{
    public class GameCtrl : MonoBehaviour
    {
        public UICtrl UICtrl;

        public PlayerInput PlayerInput;

        public List<GameObject> prefabs;
        
        /// <summary>
        /// 存放当前场上存在的棋子
        /// </summary>
        private List<GameObject> GameObjects = new List<GameObject>();

        /// <summary>
        /// 存放红旗所有步骤的栈
        /// </summary>
        private Stack<Step> RedSteps = new Stack<Step>();

        /// <summary>
        /// 存放黑棋所有步骤的栈
        /// </summary>
        private Stack<Step> BlackSteps = new Stack<Step>();

        /// <summary>
        /// 当前棋手
        /// </summary>
        private Camp currentCamp;

        void Start()
        {
            GameStart();
        }
        
        private void GameStart()
        {
            RedSteps.Clear();
            BlackSteps.Clear();
            GameObjects.Clear();

            int row = Map.map.GetLength(0);
            int col = Map.map.GetLength(1);

            for (int i = 0; i < row; ++i)//行，坐标Y
            {
                for (int j = 0; j < col; j++)//列，坐标X
                {
                    int index = Map.map[i,j];
                    if (index != 0)
                    {
                        GameObject g = Instantiate(prefabs[index - 1]);
                        g.transform.parent = this.transform;
                        g.transform.localPosition = new Vector3(j, i, 0);

                        GameObjects.Add(g);
                    }
                }
            }

            UICtrl.ChangeRurn(Camp.Black);
            currentCamp = Camp.Black;
        }
        
        public bool Move(Camp playerCamp, Qizi qizi1, Qizi qizi2,DPoint dPoint)
        {
            bool canMove = false;//移动操作是否成功
            DPoint point = Map.GetCenter(qizi1.transform.position);
            switch (qizi1.Name)
            {
                case Name.BING:
                    canMove = MoveBing(qizi1, point, qizi2, dPoint);
                    break;
                case Name.PAO:
                    canMove = MovePao(qizi1, point, qizi2, dPoint);
                    break;
                case Name.JU:
                    canMove = MoveJu(qizi1, point, qizi2, dPoint);
                    break;
                case Name.MA:
                    canMove = MoveMa(qizi1, point, qizi2, dPoint);
                    break;
                case Name.XIANG:
                    canMove = MoveXiang(qizi1, point, qizi2, dPoint);
                    break;
                case Name.SHI:
                    canMove = MoveShi(qizi1, point, qizi2, dPoint);
                    break;
                case Name.JIANG:
                    canMove = MoveJiang(qizi1, point, qizi2, dPoint);
                    break;
            }
            if (canMove)
            {
                qizi1.transform.position = new Vector3(dPoint.X, dPoint.Y, 0);
                if (qizi2 != null)
                {
                    qizi2.gameObject.SetActive(false);
                    GameObjects.Remove(qizi2.gameObject);
                }

                PlayerInput.Change();

                Record(playerCamp, new Step(qizi1, qizi2, point, dPoint));//将此步进栈

                currentCamp = (Camp)(-(int)playerCamp);
                UICtrl.ChangeRurn(currentCamp);

                bool win = JudgeWinner(qizi2);
                if (win)
                {
                    GameOver();
                }
            }
            return canMove;
        }

        #region 各类棋子走法

        /// <summary>
        /// 拱卒
        /// </summary>
        /// <param name="qizi1"></param>
        /// <param name="origin"></param>
        /// <param name="qizi2"></param>
        /// <param name="dist"></param>
        /// <returns></returns>
        bool MoveBing(Qizi qizi1, DPoint origin, Qizi qizi2, DPoint dist)
        {
            //只能移动一格；过河前不能左右移动，过河后可以；不能后退
            if (qizi1.Camp == Camp.Black)
            {
                if ((qizi1.Row > 4 && DPoint.SqrtDistance(origin, dist) == 1 && origin.Y - dist.Y != 1)
                    || (qizi1.Row < 5 && origin.Y - dist.Y == -1))
                    return true;
            }
            if (qizi1.Camp == Camp.Red)
            {
                if ((qizi1.Row < 5 && DPoint.SqrtDistance(origin, dist) == 1 && origin.Y - dist.Y != -1)
                       || (qizi1.Row > 4 && origin.Y - dist.Y == 1))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 隔山炮
        /// </summary>
        /// <param name="qizi1"></param>
        /// <param name="origin"></param>
        /// <param name="qizi2"></param>
        /// <param name="dist"></param>
        /// <returns></returns>
        bool MovePao(Qizi qizi1, DPoint origin, Qizi qizi2, DPoint dist)
        {
            int n = -1;
            if (origin.X == dist.X)//竖着
            {
                int col = origin.X + 4;

                n = GetQiziCountsAtCol(col, origin.Y + 6, dist.Y + 6);
            }

            if (origin.Y == dist.Y)//横着
            {
                int row = origin.Y + 6;

                n = GetQiziCountsAtRow(row, origin.X + 4, dist.X + 4);
            }

            if ((qizi2 == null && n == 0) || (qizi2 != null && n == 1))//无障碍直行，或者隔子攻击
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 横冲直撞 車
        /// </summary>
        /// <param name="qizi1"></param>
        /// <param name="origin"></param>
        /// <param name="qizi2"></param>
        /// <param name="dist"></param>
        /// <returns></returns>
        bool MoveJu(Qizi qizi1, DPoint origin, Qizi qizi2, DPoint dist)
        {
            int n = -1;
            if (origin.X == dist.X)//竖着
            {
                int col = qizi1.Col;

                n = GetQiziCountsAtCol(col, qizi1.Row, dist.Y + 6);
            }

            if (origin.Y == dist.Y)//横着
            {
                int row = qizi1.Row;

                n = GetQiziCountsAtCol(row, qizi1.Col, dist.X + 4);
            }

            if (n == 0)//无障碍直行
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 拐脚马
        /// </summary>
        /// <param name="qizi1"></param>
        /// <param name="origin"></param>
        /// <param name="qizi2"></param>
        /// <param name="dist"></param>
        /// <returns></returns>
        bool MoveMa(Qizi qizi1, DPoint origin, Qizi qizi2, DPoint dist)
        {
            if ((origin.X - dist.X == 2 && Mathf.Abs(origin.Y - dist.Y) == 1 && !ExistQiziAtColRow(qizi1.Row, qizi1.Col - 1))
                || (origin.X - dist.X == -2 && Mathf.Abs(origin.Y - dist.Y) == 1 && !ExistQiziAtColRow(qizi1.Row, qizi1.Col + 1))
                || (Mathf.Abs(origin.X - dist.X) == 1 && origin.Y - dist.Y == -2 && !ExistQiziAtColRow(qizi1.Row + 1, qizi1.Col))
                || (Mathf.Abs(origin.X - dist.X) == 1 && origin.Y - dist.Y == 2 && !ExistQiziAtColRow(qizi1.Row - 1, qizi1.Col))
            )
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 田字象，不过河
        /// </summary>
        /// <param name="qizi1"></param>
        /// <param name="origin"></param>
        /// <param name="qizi2"></param>
        /// <param name="dist"></param>
        /// <returns></returns>
        bool MoveXiang(Qizi qizi1, DPoint origin, Qizi qizi2, DPoint dist)
        {
            if ((origin.X - dist.X == 2 && origin.Y - dist.Y == 2 && !ExistQiziAtColRow(qizi1.Row - 1, qizi1.Col - 1))
                   || (origin.X - dist.X == 2 && origin.Y - dist.Y == -2 && !ExistQiziAtColRow(qizi1.Row + 1, qizi1.Col - 1))
                   || (origin.X - dist.X == -2 && origin.Y - dist.Y == 2 && !ExistQiziAtColRow(qizi1.Row - 1, qizi1.Col + 1))
                   || (origin.X - dist.X == -2 && origin.Y - dist.Y == -2 && !ExistQiziAtColRow(qizi1.Row + 1, qizi1.Col + 1))
               )
            {
                if ((dist.Y + 6 <= 4 && qizi1.Camp == Camp.Black) || (dist.Y + 6 >= 5 && qizi1.Camp == Camp.Red))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 九宫格斜士
        /// </summary>
        /// <param name="qizi1"></param>
        /// <param name="origin"></param>
        /// <param name="qizi2"></param>
        /// <param name="dist"></param>
        /// <returns></returns>
        bool MoveShi(Qizi qizi1, DPoint origin, Qizi qizi2, DPoint dist)
        {
            int col = dist.X + 4;
            int row = dist.Y + 6;
            if (DPoint.SqrtDistance(origin, dist) == 2)
            {
                if (col >= 3 && col <= 5)//列数限制
                {
                    //行数限制
                    if ((qizi1.Camp == Camp.Black && row <= 2) || (qizi1.Camp == Camp.Red && row >= 7))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 九宫格守门将
        /// </summary>
        /// <param name="qizi1"></param>
        /// <param name="origin"></param>
        /// <param name="qizi2"></param>
        /// <param name="dist"></param>
        /// <returns></returns>
        bool MoveJiang(Qizi qizi1, DPoint origin, Qizi qizi2, DPoint dist)
        {
            int col = dist.X + 4;
            int row = dist.Y + 6;
            if (DPoint.SqrtDistance(origin, dist) == 1)
            {
                if (col >= 3 && col <= 5)//列数限制
                {
                    //行数限制
                    if ((qizi1.Camp == Camp.Black && row <= 2) || (qizi1.Camp == Camp.Red && row >= 7))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private int GetQiziCountsAtCol(int col,int row1,int row2)
        {
            int count = 0;
            int rowMax = Mathf.Max(row1, row2);
            int rowMin = Mathf.Min(row1, row2);
            for (int i = 0; i < GameObjects.Count; i++)
            {
                Qizi qizi = GameObjects[i].GetComponent<Qizi>();
                if (qizi.Col == col && qizi.Row > rowMin && qizi.Row < rowMax)
                {
                    count++;
                }
            }
            return count;
        }

        private int GetQiziCountsAtRow(int row, int col1, int col2)
        {
            int count = 0;
            int colMax = Mathf.Max(col1, col2);
            int colMin = Mathf.Min(col1, col2);
            for (int i = 0; i < GameObjects.Count; ++i)
            {
                Qizi qizi = GameObjects[i].GetComponent<Qizi>();
                if (qizi.Row == row && qizi.Col > colMin && qizi.Col < colMax)
                {
                    count++;
                }
            }
            return count;
        }

        private bool ExistQiziAtColRow(int row, int col)
        {
            for (int i = 0; i < GameObjects.Count; i++)
            {
                Qizi qizi = GameObjects[i].GetComponent<Qizi>();
                if (qizi.Row == row &&qizi.Col==col)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        /// <summary>
        /// 记录走棋步骤
        /// </summary>
        private void Record(Camp camp, Step step)
        {
            switch (camp)
            {
                case Camp.Red:
                    RedSteps.Push(step);
                    break;
                case Camp.Black:
                    BlackSteps.Push(step);
                    break;
            }
        }

        /// <summary>
        /// 悔棋
        /// </summary>
        public void Recall()
        {
            Step stepB, stepR;
            if (RedSteps.Count == 0 || BlackSteps.Count == 0) return;
            switch (currentCamp)
            {
                case Camp.Red://红棋悔棋，先悔一步黑棋，再悔一步红棋
                    stepB = BlackSteps.Pop();
                    stepB.move.transform.position = new Vector3(stepB.origin.X, stepB.origin.Y, 0);
                    if (stepB.destory != null)
                    {
                        stepB.destory.transform.position = new Vector3(stepB.dist.X, stepB.dist.Y, 0);
                        stepB.destory.gameObject.SetActive(true);
                        GameObjects.Add(stepB.destory.gameObject);
                    }

                    stepR = RedSteps.Pop();
                    stepR.move.transform.position = new Vector3(stepR.origin.X, stepR.origin.Y, 0);
                    if (stepR.destory != null)
                    {
                        stepR.destory.transform.position = new Vector3(stepR.dist.X, stepR.dist.Y, 0);
                        stepR.destory.gameObject.SetActive(true);
                        GameObjects.Add(stepR.destory.gameObject);
                    }
                    break;
                case Camp.Black://黑棋悔棋，先悔一步红棋，再悔一步黑棋
                    stepR = RedSteps.Pop();
                    stepR.move.transform.position = new Vector3(stepR.origin.X, stepR.origin.Y, 0);
                    if (stepR.destory != null)
                    {
                        stepR.destory.transform.position = new Vector3(stepR.dist.X, stepR.dist.Y, 0);
                        stepR.destory.gameObject.SetActive(true);
                        GameObjects.Add(stepR.destory.gameObject);
                    }

                    stepB = BlackSteps.Pop();
                    stepB.move.transform.position = new Vector3(stepB.origin.X, stepB.origin.Y, 0);
                    if (stepB.destory != null)
                    {
                        stepB.destory.transform.position = new Vector3(stepB.dist.X, stepB.dist.Y, 0);
                        stepB.destory.gameObject.SetActive(true);
                        GameObjects.Add(stepB.destory.gameObject);
                    }
                    break;
            }
        }

        /// <summary>
        /// UI通知
        /// </summary>
        /// <param name="camp"></param>
        /// <param name="dPoint"></param>
        public void SelectBox(Camp camp, DPoint dPoint)
        {
            UICtrl.ChoosePosition(camp, dPoint);
        }
        
        private void GameOver()
        {
            Debug.Log("游戏结束");
            UICtrl.GameOver(winner);
            PlayerInput.enabled = false;
        }

        private Camp winner;
        private bool JudgeWinner(Qizi destoryQizi)
        {
            if (destoryQizi == null)
                return false;

            if (destoryQizi.Name == Name.JIANG)
            {
                winner = (Camp)(-(int)destoryQizi.Camp);

                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 操作步骤
    /// </summary>
    public struct Step
    {
        public Qizi move;
        public Qizi destory;
        public DPoint origin;
        public DPoint dist;

        public Step(Qizi qizi1, Qizi qizi2, DPoint dPoint1, DPoint dPoint2)
        {
            move = qizi1;
            destory = qizi2;
            origin = dPoint1;
            dist = dPoint2;
        }
    }
}