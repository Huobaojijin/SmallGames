using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DXiangqi
{
    public class PlayerInput : MonoBehaviour
    {
        public Camp camp;

        public GameCtrl GameCtrl;

        public Qizi choose;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    dPoint1 = Map.GetCenter(vector);

                    switch (hit.collider.tag)
                    {
                        case "Qizi":
                            Qizi qizi = hit.collider.GetComponent<Qizi>();
                            if (qizi.Camp == camp)//同颜色，替换
                            {
                                choose = qizi;
                                GameCtrl.SelectBox(camp, dPoint1);
                            }
                            else
                            {
                                if (choose == null) return;

                                GameCtrl.Move(camp, choose, qizi, dPoint1);
                            }
                            break;
                        case "QiPan":
                            GameCtrl.SelectBox(camp, dPoint1);
                            if (choose == null) return;

                            GameCtrl.Move(camp, choose, null, dPoint1);
                            break;
                    }
                }
            }
        }

        public DPoint dPoint1;
        public DPoint dPoint2;

        public void Change()
        {
            choose = null;
            camp = (Camp)(- (int)camp);
        }
    }
    
    public struct DPoint
    {
        public int X;
        public int Y;

        public DPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        /// <summary>
        /// 返回两点距离的平方
        /// </summary>
        /// <param name="dPoint1"></param>
        /// <param name="dPoint2"></param>
        /// <returns></returns>
        public static int SqrtDistance(DPoint dPoint1, DPoint dPoint2)
        {
            return (dPoint1.X - dPoint2.X) * (dPoint1.X - dPoint2.X) + (dPoint1.Y - dPoint2.Y) * (dPoint1.Y - dPoint2.Y);
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", X, Y);
        }
    }
}