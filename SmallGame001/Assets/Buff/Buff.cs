using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Buff
{
    public BaseCharacter character;

    public BuffType buffType;

    public float duration;

    public Buff(BaseCharacter _character, BuffType _buffType, float _duration)
    {
        character = _character;
        buffType = _buffType;
        duration = _duration;
    }

    public virtual void OnAdd() { }

    public virtual void OnUpdate() { }

    public virtual void OnRemove() { }
}

public enum BuffType
{
    Blood,
    Frozen,
    Slow
}
