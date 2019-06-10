using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace XDouShouQi
{
    public class Animal : MonoBehaviour
    {
        /// <summary>
        /// 所属阵营
        /// </summary>
        public Camp camp;

        /// <summary>
        /// 战力指数
        /// </summary>
        public int battle = 0;
        
        [EnumFlags]
        public Location matchTerrain;

        public bool front = false;

        private DOTweenAnimation tweenAnimation;

        private void Start()
        {
            front = false;
            tweenAnimation = GetComponent<DOTweenAnimation>();
        }

        public void TurnOver()
        {
            tweenAnimation.DOPlayForward();
            front = true;
        }

        /// <summary>
        /// 是否能占领此地
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool IsMatchLocation(Location location)
        {
            int index = 1 << (int)location;
            int collection = (int)matchTerrain;
            if ((collection & index) == index)
            {
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 阵营
    /// </summary>
    public enum Camp
    {
        Hong = 0,
        Lan = 1
    }
}