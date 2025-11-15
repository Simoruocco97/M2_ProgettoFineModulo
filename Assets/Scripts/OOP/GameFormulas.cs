using UnityEngine;

public static class GameFormulas
{

    public static bool HasElementAdvantage(ELEMENT attackElement, Hero defender)
    {
        if (attackElement == defender.Weakness && attackElement != ELEMENT.NONE)  //ho perso un sacco di tempo per capire perchè NONE fosse superefficace..
        {
            Debug.Log($"WEAKNESS! {defender.GetName()} e' debole a {attackElement}.");
            return true;
        }
        return false;
    }

    public static bool HasElementDisvantage(ELEMENT attackElement, Hero defender)
    {
        if (attackElement == defender.Resistance && attackElement != ELEMENT.NONE)
        {
            Debug.Log($"RESIST! {defender.GetName()} e' resistente a {attackElement}.");
            return true;
        }
        return false;
    }

    public static float EvaluateElementalModifier(ELEMENT attackElement, Hero defender)
    {
        if (HasElementDisvantage(attackElement, defender))
        {
            return 0.5f;
        }
        else if (HasElementAdvantage(attackElement, defender))
        {
            return 1.5f;
        }
        else
        {
            return 1f;
        }
    }

    public static bool HasHit(Stats attacker, Stats defender)
    {
        int hitChance = attacker.aim - defender.eva;
        hitChance = Mathf.Clamp(hitChance, 5, 95);  //trovato sulla lib di Mathf. in questo modo per quanto possa essere bassa 

        int random = Random.Range(0, 100);          //la precisione c'è il 5% di hitchance e viceversa almeno un 5% di probabilità di missare.
        if (random > hitChance)
        {
            Debug.Log("MISS");
            return false;
        }
        else
        {
            return true;
        }
    }

    public static bool IsCrit(int critValue)
    {
        int random = Random.Range(0, 100);
        if (random < critValue)
        {
            Debug.Log("CRIT");
            return true;
        }
        else
        {
            return false;
        }
    }

    public static int CalculateDamage(Hero attacker, Hero defender)
    {
        Stats attackerTot = Stats.Sum(attacker.BaseStats, attacker.Weapon.BonusStats);
        Stats defenderTot = Stats.Sum(defender.BaseStats, defender.Weapon.BonusStats);

        float attackerDmg = attackerTot.atk;
        float modifier;
        int damage;

        if (IsCrit(attackerTot.crt))
        {
            attackerDmg *= 2;
        }

        modifier = EvaluateElementalModifier(attacker.Weapon.Elem, defender);

        attackerDmg *= modifier;

        if (attacker.Weapon.DmgType == Weapon.DAMAGE_TYPE.PHYSICAL)
        {
            damage = (int)attackerDmg - defenderTot.def;
        }
        else // quindi se (attacker.Weapon.DmgType == Weapon.DAMAGE_TYPE.MAGICAL)
        {
            damage = (int)attackerDmg - defenderTot.res;
        }

        if (damage < 0)
        {
            damage = 0;
        }

        Debug.Log($"{attacker.GetName()} colpisce {defender.GetName()} infliggendo {damage} danni!");
        return damage;
    }

    //Funzioni per tener pulito Update()

    public static void FightCheck(Hero firstHero, Hero secondHero)
    {
        while (firstHero.IsAlive() && secondHero.IsAlive())
        {
            Hero[] order = SpdCheck(firstHero, secondHero);
            Hero first = order[0];
            Hero second = order[1];
            
            Attack(first, second);

            if (second.IsAlive())
            {
                Attack(second, first);
            }

        }
    }

    private static Hero[] SpdCheck(Hero firstHero, Hero secondHero)
    {
        Hero first;
        Hero second;

        if (firstHero.BaseStats.spd >= secondHero.BaseStats.spd)
        {
            first = firstHero;
            second = secondHero;
        }
        else
        {
            first = secondHero;
            second = firstHero;
        }
        Hero[] HeroesInOrder = new Hero[2];
        HeroesInOrder[0] = first;
        HeroesInOrder[1] = second;
        return HeroesInOrder;
    }

    private static void Attack(Hero attacker, Hero defender)
    {
        Debug.Log($"{attacker.GetName()} attacca {defender.GetName()}");
        defender.TakeDamage(CalculateDamage(attacker, defender));

        if (!defender.IsAlive())
        {
            Debug.Log($"{defender.GetName()} e' morto. {attacker.GetName()} vince il duello!");
        }
    }
}