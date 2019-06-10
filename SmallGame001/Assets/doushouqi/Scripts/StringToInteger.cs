using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XDouShouQi 
{
    public class StringToInteger
    {
        public static int GetNumberInt(string str)
        {
            string result = System.Text.RegularExpressions.Regex.Replace(str, @"[^0-9]+", "");

            return int.Parse(result);
        } 
    }
}