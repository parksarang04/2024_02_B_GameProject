using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{

    public PlayerTarget player;
    public EnemyTarget enemy;

    public List<EnemyTarget> enemyTargets;

    public SKill<ISKillTarget, DamageEffect> firball;
    public SKill<PlayerTarget, HealEffect> healSpell;
    public SKill<ISKillTarget, DamageEffect>multiTargetSkill;
    // Start is called before the first frame update
    void Start()
    {
        firball = new SKill<ISKillTarget, DamageEffect>("Fireball", new DamageEffect(20));
        healSpell = new SKill<PlayerTarget, HealEffect>("Heal", new HealEffect(30));
        multiTargetSkill = new SKill<ISKillTarget, DamageEffect>("AoE Attack", new DamageEffect(10));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Alpha1))
        {
            firball.Use(enemy);
        }
        if(Input.GetKeyUp(KeyCode.Alpha2))
        {
            healSpell.Use(player);
        }
        if(Input.GetKeyUp(KeyCode.Alpha3))
        {
            foreach (var target in enemyTargets)
            {
                multiTargetSkill.Use(target);
            }
            
        }
    }
}
