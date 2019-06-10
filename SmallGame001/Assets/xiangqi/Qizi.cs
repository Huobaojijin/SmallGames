using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DXiangqi
{
    public class Qizi : MonoBehaviour
    {
        /// <summary>
        /// 阵营
        /// </summary>
        public Camp Camp;

        /// <summary>
        /// 名字
        /// </summary>
        public Name Name;

        /// <summary>
        /// 是否能越过楚河汉界
        /// </summary>
        public bool canRiver;
        
        /// <summary>
        /// 所在行
        /// </summary>
        public int Row
        {
            get
            {
                return Map.GetCenter(transform.localPosition).Y;
            }
        }

        /// <summary>
        /// 所在列
        /// </summary>
        public int Col
        {
            get
            {
                return Map.GetCenter(transform.localPosition).X;
            }
        }

        private void OnMouseOver()
        {
            transform.localScale = Vector3.one * 1.1f;
        }

        private void OnMouseExit()
        {
            transform.localScale = Vector3.one;
        }
    }

    public enum Camp
    {
        Red = -1,
        Black = 1
    }

    public enum Name
    {
        BING,
        PAO,
        JU,
        MA,
        XIANG,
        SHI,
        JIANG
    }
}