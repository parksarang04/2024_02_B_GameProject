using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//구체적인 타겟 클래스 
public class PlayerTarget : MonoBehaviour , ISkillTarget
{
    public int Health { get; set; } = 100;

    public void ApplyEffect(ISkillEffect effect)
    {
        effect.Apply(this);
    }
}
