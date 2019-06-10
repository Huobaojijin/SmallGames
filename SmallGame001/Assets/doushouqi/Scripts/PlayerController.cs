using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XDouShouQi
{
    public class PlayerController : MonoBehaviour
    {
        public Camp myCamp;

        public Cell firstCell;
        public Cell secondCell;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null)
                    {
                        Debug.Log("" + myCamp.ToString() + "------>" + hit.collider);
                        Cell hitCell = hit.collider.GetComponent<Cell>();
                        //点击到了非空格
                        if (hitCell.son != null)
                        {
                            Animal animal = hitCell.son.GetComponent<Animal>();
                            if (!animal.front)//翻牌
                            {
                                animal.TurnOver();

                                ClearChoose();
                                TurnEndEvent(myCamp);
                            }
                            else//战斗
                            {
                                if (firstCell == null)
                                {
                                    if (animal.camp == this.myCamp)
                                    {
                                        firstCell = hitCell;
                                    }
                                }
                                else
                                {
                                    Animal firstAnimal = firstCell.son.GetComponent<Animal>();
                                    if (firstAnimal.camp == animal.camp)//重复阵营，则替换
                                    {
                                        firstCell = hitCell;
                                    }
                                    else
                                    {
                                        IndexVector first = firstCell.IndexVector;
                                        IndexVector second = hitCell.IndexVector;
                                        int distance = (first.X - second.X) * (first.X - second.X) + (first.Y - second.Y) * (first.Y - second.Y);//判断距离，是否是相邻的格子
                                        Debug.Log(distance);
                                        if (distance == 1)
                                        {
                                            Location location = hitCell.location;

                                            if (firstAnimal.IsMatchLocation(location))
                                            {
                                                Animal secondAnimal = hitCell.son.GetComponent<Animal>();

                                                //战斗，战力高的一方可以打败战力低的一方
                                                //特殊情况，老鼠0可以吃掉大象7,大象打不过老鼠
                                                int difference = firstAnimal.battle - secondAnimal.battle;
                                                //吃掉，胜
                                                if (difference > 0 && difference != 7 || difference == -7)
                                                {
                                                    //销毁掉输家
                                                    hitCell.son.SetActive(false);
                                                    hitCell.son = null;

                                                    //赢家转移
                                                    GameObject firstSon = firstCell.son;
                                                    firstSon.transform.parent = hitCell.transform;
                                                    firstSon.transform.localPosition = Vector3.zero;
                                                    hitCell.son = firstSon;
                                                    firstCell.son = null;

                                                    if (myCamp == Camp.Hong)
                                                    {
                                                        BattleProcessEvent(0, 1);
                                                    }
                                                    else
                                                    {
                                                        BattleProcessEvent(1, 0);
                                                    }
                                                }
                                                //同归于尽，平
                                                else if (difference == 0)
                                                {
                                                    hitCell.son.SetActive(false);
                                                    hitCell.son = null;

                                                    firstCell.son.SetActive(false);
                                                    firstCell.son = null;

                                                    BattleProcessEvent(1, 1);
                                                }
                                                //打不过，败
                                                else
                                                {
                                                    return;
                                                }

                                                ClearChoose();
                                                TurnEndEvent(myCamp);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //点击到了空格
                        else
                        {
                            if (firstCell == null)//第一个选择不能为空
                                return;

                            //第二个选择，移动到空格子上
                            Animal firstAnimal = firstCell.son.GetComponent<Animal>();
                            IndexVector first = firstCell.IndexVector;
                            IndexVector second = hitCell.IndexVector;
                            int distance = (first.X - second.X) * (first.X - second.X) + (first.Y - second.Y) * (first.Y - second.Y);//判断距离，是否是相邻的格子

                            if (distance == 1)
                            {
                                Location location = hitCell.location;

                                if (firstAnimal.IsMatchLocation(location))//可以占领该空格
                                {
                                    //移动到空格，重置其他
                                    GameObject animal = firstCell.son;
                                    animal.transform.parent = hitCell.transform;
                                    animal.transform.localPosition = Vector3.zero;
                                    hitCell.son = animal;
                                    firstCell.son = null;

                                    ClearChoose();
                                    TurnEndEvent(myCamp);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ClearChoose()
        {
            firstCell = null;
            secondCell = null;
        }

        private bool InvaildOperation()
        {

            return false;
        }

        public delegate void TurnEndDel(Camp camp);
        public event TurnEndDel TurnEndEvent;

        public delegate void BattleProcess(int hong, int lan);
        public event BattleProcess BattleProcessEvent;
    }
}