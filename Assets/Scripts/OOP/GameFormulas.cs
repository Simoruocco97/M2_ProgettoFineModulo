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

        int damage;
        float modifier;

        if (attacker.Weapon.DmgType == Weapon.DAMAGE_TYPE.PHYSICAL)
        {
            damage = attackerTot.atk - defenderTot.def;
        }
        else // quindi se (attacker.Weapon.DmgType == Weapon.DAMAGE_TYPE.MAGICAL)
        {
            damage = attackerTot.atk - defenderTot.res;
        }

        if (damage < 0)
        {
            damage = 0;
        }

        modifier = EvaluateElementalModifier(attacker.Weapon.Elem, defender);
        damage = (int)(damage * modifier);

        if (IsCrit(attackerTot.crt))
        {
            damage *= 2;
        }
        Debug.Log($"{attacker.GetName()} colpisce {defender.GetName()} infliggendo {damage} danni!");
        return damage;
    }

    //Funzioni per tener pulito Update()

    public static void FightCheck(Hero hero, Hero hero2)
    {
        while (hero.IsAlive() && hero2.IsAlive())
        {
            Hero[] order = SpdCheck(hero, hero2);
            Hero first = order[0];
            Hero second = order[1];

            Attack(first, second);

            if (second.IsAlive())
            {
                Attack(second, first);
            }
        }
    }

    private static Hero[] SpdCheck(Hero hero, Hero hero2)
    {
        Hero first;
        Hero second;

        if (hero.BaseStats.spd >= hero2.BaseStats.spd)
        {
            first = hero;
            second = hero2;
        }
        else
        {
            first = hero2;
            second = hero;
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



//vecchio codice, poco legibile e difficile da modificare

//if (hero.BaseStats.spd > hero2.BaseStats.spd)       //caso in cui attacca hero1
//{
//    Debug.Log($"{hero.GetName()} attacca per primo, {hero2.GetName()} e' pronto a difendersi!");
//    hero2.TakeDamage(CalculateDamage(hero, hero2));

//    if (!hero2.IsAlive())   //caso in cui hero2 muore e si interrompe il codice
//    {
//        Debug.Log($"{hero2.GetName()} e' morto! {hero.GetName()} vince il duello.");
//        return;
//    }

//    else if (hero2.IsAlive())    //caso in cui attacca hero2 se sopravvie
//    {
//        Debug.Log($"{hero2.GetName()} attacca {hero.GetName()} che e' pronto a difendersi!");
//        hero.TakeDamage(CalculateDamage(hero2, hero));
//    }

//}
//else
//{
//    Debug.Log($"{hero2.GetName()} attacca per primo, {hero.GetName()} e' pronto a difendersi!");    //caso in cui hero2 attacca
//    hero.TakeDamage(CalculateDamage(hero2, hero));

//    if (!hero.IsAlive())
//    {
//        Debug.Log($"{hero.GetName()} e' morto! {hero2.GetName()} vince il duello.");
//        return;
//    }    //caso in cui hero muore e si interrompe il codice

//    else if (hero.IsAlive())
//    {
//        Debug.Log($"{hero.GetName()} attacca {hero2.GetName()} che e' pronto a difendersi!");   //caso in cui hero1 attacca se sopravvive
//        hero2.TakeDamage(CalculateDamage(hero, hero2));
//    }