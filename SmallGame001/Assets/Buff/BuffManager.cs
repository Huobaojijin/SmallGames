using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public List<Buff> buffs;

    
    public void Update()
    {
        for (int i = buffs.Count - 1; i >= 0; --i)
        {
            buffs[i].OnUpdate();
        }
    }

    public void AddBuff(Buff buff)
    {
        buffs.Add(buff);
    }

    public void RemoveBuff(Buff buff)
    {
        buffs.Remove(buff);
    }
}
