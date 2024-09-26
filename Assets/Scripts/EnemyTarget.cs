using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTarget : MonoBehaviour , ISKillTarget
{
    public int Health { get;  set; } = 50;

    public void ApplyEffect(ISkillEffect effect)
    {
        effect.Apply(this);
    }
}
