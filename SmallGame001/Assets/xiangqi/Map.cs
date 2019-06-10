using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DXiangqi
{
    public class Map
    {
        public static int[,] map = new int[,]
        {
            { 3,4,5,6,7,6,5,4,3},
            { 0,0,0,0,0,0,0,0,0},
            { 0,2,0,0,0,0,0,2,0},
            { 1,0,1,0,1,0,1,0,1},
            { 0,0,0,0,0,0,0,0,0},
            { 0,0,0,0,0,0,0,0,0},
            { 8,0,8,0,8,0,8,0,8},
            { 0,9,0,0,0,0,0,9,0},
            { 0,0,0,0,0,0,0,0,0},
            { 10,11,12,13,14,13,12,11,10},
        }; 

        public static DPoint GetCenter(Vector3 vector3)
        {
            float x = vector3.x;
            float y = vector3.y;

            int xr = Mathf.CeilToInt(x);
            int yr = Mathf.CeilToInt(y);

            if (xr- x > 0.5f)
            {
                xr = xr - 1;
            }

            if (yr - y > 0.5f)
            {
                yr = yr - 1;
            }

            return new DPoint(xr, yr);
        }
    }
}