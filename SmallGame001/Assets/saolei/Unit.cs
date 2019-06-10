using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DSaoLei
{
    public class Unit : MonoBehaviour
    {
        public bool isLei;

        public int roundLeiNumber;
        
        [HideInInspector]
        public bool isShowed = false;

        [HideInInspector]
        public int Row;

        [HideInInspector]
        public int Col;

        public GameObject bg;

        [HideInInspector]
        public UnitState UnitState = UnitState.Empty;

        public void HideBg()
        {
            bg.SetActive(false);
        }
    }

    public enum UnitState
    {
        Empty,
        Flag,
        Doubt
    }
}