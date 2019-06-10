using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XDouShouQi
{
    public class Cell : MonoBehaviour
    {
        /// <summary>
        /// 场景属性：普通还是地形
        /// </summary>
        public Location location;

        /// <summary>
        /// 附属子物体
        /// </summary>
        public GameObject son;

        public IndexVector IndexVector { get; private set; }

        private void Start()
        {
            string name = transform.name;
            int num = StringToInteger.GetNumberInt(name);

            IndexVector = new IndexVector
            {
                X = num / 10,
                Y = num % 10
            };
        }
    }
    
    public enum Location
    {
        Normal,
        San,
        He,
        Shu,
        Jing
    }

    public struct IndexVector
    {
        public int X;
        public int Y;
    }
}