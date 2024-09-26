using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//스킬 타켓 인터페이스
public interface ISKillTarget
{
    void ApplyEffect(ISkillEffect effect);
}

public interface ISkillEffect
{ 
    void Apply(ISKillTarget target);
}

//구체적인 효과 클래스
public class DamageEffect : ISkillEffect
{
    public int Damage { get; private set; }

    public DamageEffect(int damage)
    {
        Damage = damage;
    }

    public void Apply(ISKillTarget target)
    {
        if (target is PlayerTarget playerTarget)
        {
            playerTarget.Health -= Damage;
            Debug.Log($"Player took {Damage} damage. Remaining health : {playerTarget.Health}");
        }
        else if (target is EnemyTarget enemyTarget)
        {
            enemyTarget.Health -= Damage;
            Debug.Log($"Player took {Damage} damage. Remaining health : {enemyTarget.Health}");
        }
    }
}


public class HealEffect : ISkillEffect
{
    public int HealAmount { get; private set; }

    public HealEffect(int damage)
    {
        HealAmount = damage;
    }

    public void Apply(ISKillTarget target)
    {
        if (target is PlayerTarget playerTarget)
        {
            playerTarget.Health += HealAmount;
            Debug.Log($"Player Healed {HealAmount}. Remaining health : {playerTarget.Health}");
        }
        else if (target is EnemyTarget enemyTarget)
        {
            enemyTarget.Health -= HealAmount;
            Debug.Log($"Player Healed for {HealAmount}. Remaining health : {enemyTarget.Health}");
        }
    }
}

//제네릭 스킬 클래스
public class SKill<TTarget, TEffect>
    where TTarget : ISKillTarget
    where TEffect : ISkillEffect
{
    public string Name { get; private set; }
    public TEffect Effect { get; private set; }

    public SKill(string name, TEffect effect)
    {
        Name = name;
        Effect = effect;
    }
    public void Use(TTarget target)
    {
        Debug.Log($"Using skill:{Name}");
        target.ApplyEffect(Effect);
    }
}