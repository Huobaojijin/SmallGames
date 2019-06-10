using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace X2048
{
    public class PlayerCtrl : MonoBehaviour
    {
        GameCtrl gameCtrl;
        // Use this for initialization
        void Start()
        {
            gameCtrl = GetComponent<GameCtrl>();
        }

        bool isMove = false;
        bool isFlag = false;
        float startTime = 0;
        Vector3 startPoint;
        Vector3 livePoint;

        Direction direction = Direction.DOWN;
        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isMove = true;
                startPoint = Input.mousePosition;
                isFlag = true;
                startTime = Time.fixedTime;
            }
            else if(Input.GetMouseButtonUp(0))
            {
                isMove = false;
            }

            if (isMove)
            {
                livePoint = Input.mousePosition;
                if (isFlag == false) return;
                if (Time.fixedTime - startTime > 3) return;//超时

                Vector3 v = livePoint - startPoint;
                double len = v.magnitude;

                if (len < 30) return;

                var degree = Mathf.Rad2Deg * Mathf.Atan2(v.x, v.y);

                if (degree < -45 && degree > -135)
                {
                    direction = Direction.LEFT;
                }
                else if (degree > 45 && degree < 135)
                {
                    direction = Direction.RIGHT;
                }
                else if (degree >= -45 && degree <= 45)
                {
                    direction = Direction.UP;
                }
                else
                {
                    direction = Direction.DOWN;
                }
                isFlag = false;

                gameCtrl.MoveGrid(direction);
            }
        }

        private void Check(Vector3 point)
        {

        }
    }

    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
}